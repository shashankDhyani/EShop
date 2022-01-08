using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShop.Infrastructure.EventBus;
using EShop.Infrastructure.Mongo;
using EShop.Inventory.Api.Handlers;
using EShop.Inventory.DataProvider.Repository;
using EShop.Inventory.DataProvider.Services;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EShop.Inventory.Api
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
            services.AddMongoDb(Configuration);
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IInventoryService, InventoryService>();
            var rabbitmqOption = new RabbitMqOption();
            Configuration.GetSection("rabbitmq").Bind(rabbitmqOption);

            services.AddMassTransit(x => {
                x.AddConsumer<AllocateProductConsumer>();
                x.AddConsumer<ReleaseProductConsumer>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(rabbitmqOption.ConnectionString), hostconfig =>
                    {
                        hostconfig.Username(rabbitmqOption.Username);
                        hostconfig.Password(rabbitmqOption.Password);
                    });

                    cfg.ReceiveEndpoint("allocate_product", ep => {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(retryConfig => { retryConfig.Interval(2, 100); });
                        ep.ConfigureConsumer<AllocateProductConsumer>(provider);
                    });
                    cfg.ReceiveEndpoint("release_product", ep => {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(retryConfig => { retryConfig.Interval(2, 100); });
                        ep.ConfigureConsumer<ReleaseProductConsumer>(provider);
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

            var dbInitializer = app.ApplicationServices.GetService<IDatabaseInitializer>();
            dbInitializer.InitializeAsync();
        }
    }
}
