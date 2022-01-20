using EShop.Cart.Api.Handlers;
using EShop.Cart.DataProvider.Repository;
using EShop.Cart.DataProvider.Services;
using EShop.Infrastructure.EventBus;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace EShop.Cart.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddStackExchangeRedisCache((cfg) =>
            {
                cfg.Configuration = $"{Configuration["Redis:Host"]}:{Configuration["Redis:Port"]}";
            });

            services.AddSingleton<ICartRepository, CartRepository>();
            services.AddSingleton<ICartService, CartService>();

            var options = new RabbitMqOption();
            Configuration.GetSection("rabbitmq").Bind(options);

            services.AddMassTransit(x => {
                x.AddConsumer<AddCartItemConsumer>();
                x.AddConsumer<RemoveCartItemConsumer>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg=> {
                    cfg.Host(new Uri(options.ConnectionString), hostConfig => {
                        hostConfig.Username(options.Username);
                        hostConfig.Username(options.Password);
                    });

                    cfg.ReceiveEndpoint("add_cart", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(retryConfig => { retryConfig.Interval(2, 100); });
                        ep.ConfigureConsumer<AddCartItemConsumer>(provider);
                    });

                    cfg.ReceiveEndpoint("remove_cart", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(retryConfig => { retryConfig.Interval(2, 100); });
                        ep.ConfigureConsumer<RemoveCartItemConsumer>(provider);
                    });
                }));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var busControl = app.ApplicationServices.GetService<IBusControl>();
            busControl.Start();
        }
    }
}
