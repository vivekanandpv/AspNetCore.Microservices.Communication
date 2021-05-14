using Microservice.B.Models;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.B.MQ
{
    public class RabbitMQPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly RabbitMQConfig _rabbitMQConfig;

        private readonly IConnection _connection;

        public RabbitMQPolicy(IOptions<RabbitMQConfig> options)
        {
            _rabbitMQConfig = options.Value;
            _connection = GetConnection();
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQConfig.HostName,
                UserName = _rabbitMQConfig.UserName,
                Password = _rabbitMQConfig.Password,
                Port = _rabbitMQConfig.Port,
                VirtualHost = _rabbitMQConfig.VirtualHost,
                Ssl = new SslOption
                {
                    ServerName = _rabbitMQConfig.HostName,
                    Enabled = true
                }
            };

            return factory.CreateConnection();
        }

        public IModel Create()
        {
            return _connection.CreateModel();
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }
            else
            {
                obj?.Dispose();
                return false;
            }
        }
    }
}
