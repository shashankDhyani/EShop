using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShop.Infrastructure.EventBus;
using EShop.Infrastructure.Mongo;
using EShop.Wallet.Api.Handlers;
using EShop.Wallet.DataProvider.Repository;
using EShop.Wallet.DataProvider.Services;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EShop.Wallet.Api
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

            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IWalletRepository, WalletRepository>();

            services.AddMongoDb(Configuration);

            var rabbitmqConfig = new RabbitMqOption();
            Configuration.GetSection("rabbitmq").Bind(rabbitmqConfig);

            services.AddMassTransit(x=> {
                x.AddConsumersFromNamespaceContaining<AddFundsConsumer>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg => {
                    cfg.Host(new Uri(rabbitmqConfig.ConnectionString), hostConfig =>
                    {
                        hostConfig.Username(rabbitmqConfig.Username);
                        hostConfig.Password(rabbitmqConfig.Password);
                    });

                    cfg.ReceiveEndpoint("add_funds", ep=> {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(retryConfig => { retryConfig.Interval(2, 100); });
                        ep.ConfigureConsumer<AddFundsConsumer>(provider);
                    });

                    cfg.ReceiveEndpoint("deduct_funds", ep => {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(retryConfig => { retryConfig.Interval(2, 100); });
                        ep.ConfigureConsumer<DeductFundsConsumer>(provider);
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

            app.UseHttpsRedirection();

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
