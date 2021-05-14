using Microservice.B.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.B.MQ
{
    public static class RabbitMQExtensions
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMQConfig = configuration.GetSection("RabbitMQConfig");
            services.Configure<RabbitMQConfig>(rabbitMQConfig);

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitMQPolicy>();

            services.AddSingleton<IRabbitMQManager, RabbitMQManager>();

            return services;
        }
    }
}
