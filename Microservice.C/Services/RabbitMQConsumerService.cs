using Microservice.C.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.C.Services
{
    public class RabbitMQConsumerService : BackgroundService, IDisposable
    {
        private readonly ILogger<RabbitMQConsumerService> _logger;
        private IConnection _connection;
        private IModel _channel;
        private readonly RabbitMQConfig _rabbitMQConfig;

        public RabbitMQConsumerService(ILoggerFactory loggerFactory, IOptions<RabbitMQConfig> options)
        {
            _logger = loggerFactory.CreateLogger<RabbitMQConsumerService>();
            _rabbitMQConfig = options.Value;
            InitializeMessageQueue();
        }

        private void InitializeMessageQueue()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMQConfig.HostName,
                UserName = _rabbitMQConfig.UserName,
                Password = _rabbitMQConfig.Password,
                VirtualHost = _rabbitMQConfig.VirtualHost,
                Port = _rabbitMQConfig.Port,
                Ssl = new SslOption
                {
                    ServerName = _rabbitMQConfig.HostName,
                    Enabled = true
                }
            };

            // create connection  
            _connection = factory.CreateConnection();

            // create channel  
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("ms-exchange", ExchangeType.Topic);
            _channel.QueueDeclare("ms-queue", false, false, false, null);
            _channel.QueueBind("ms-queue", "ms-exchange", "ms-b-routing", null);
            _channel.BasicQos(0, 1, false);

            _connection.ConnectionShutdown += (sender, args) =>
            {
                _logger.LogInformation($"Message queue connection shutting down...");
            };
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, args) =>
            {
                var messageString = System.Text.Encoding.UTF8.GetString(args.Body.ToArray());
                var message = JsonConvert.DeserializeObject<Message>(messageString);

                _logger.LogInformation($"Message received: {Environment.NewLine}{message.MessageTitle}{Environment.NewLine}{message.MessageBody}");

                _channel.BasicAck(args.DeliveryTag, false);
            };

            //  Also consider other events on consumer

            _channel.BasicConsume("ms-queue", false, consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
