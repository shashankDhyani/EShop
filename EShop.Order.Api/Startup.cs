using Eshop.Product.DataProvider.Repository;
using EShop.Infrastructure.Activities.RoutingActivities;
using EShop.Infrastructure.EventBus;
using EShop.Infrastructure.Mongo;
using EShop.Order.Api.Handlers;
using EShop.Order.DataProvider.Repository;
using EShop.Order.DataProvider.Services;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace EShop.Order.Api
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
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddMongoDb(Configuration);
            services.AddSwaggerGen(c=> {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {
                    Version="v1",
                    Title="EShop Order API Endpoints",
                    Description="These API Endpoints are availeble to CRUD Order related data."
                });
            });


            var rabbitmqOption = new RabbitMqOption();
            Configuration.GetSection("rabbitmq").Bind(rabbitmqOption);

            services.AddMassTransit(x => {
                x.AddConsumersFromNamespaceContaining<CreateOrderHandler>();
                x.AddActivitiesFromNamespaceContaining<RoutingActivities>();
                x.SetKebabCaseEndpointNameFormatter();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(rabbitmqOption.ConnectionString), hostconfig =>
                    {
                        hostconfig.Username(rabbitmqOption.Username);
                        hostconfig.Password(rabbitmqOption.Password);
                    });

                    cfg.ConfigureEndpoints(provider);
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

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API");
            });

            var busControl = app.ApplicationServices.GetService<IBusControl>();
            busControl.Start();

            var dbInitializer = app.ApplicationServices.GetService<IDatabaseInitializer>();
            dbInitializer.InitializeAsync();
        }
    }
}
