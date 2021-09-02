using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace EShop.Infrastructure.Mongo
{
    public static class Extension
    {
        public static void AddMongoDb(this IServiceCollection services,IConfiguration configuration)
        {
            var configSection = configuration.GetSection("mongo");

            var mongoConfig = new MongoConfig();
            configSection.Bind(mongoConfig);

            services.AddSingleton<IMongoClient>(client=> {
                return new MongoClient(mongoConfig.ConnectionString);
            });
            services.AddSingleton<IMongoDatabase>(client => {
                var mongoClient = client.GetService<IMongoClient>();
                return mongoClient.GetDatabase(mongoConfig.Database);
            });

            services.AddSingleton<IDatabaseInitializer, MongoInitializer>();
        }
    }
}
