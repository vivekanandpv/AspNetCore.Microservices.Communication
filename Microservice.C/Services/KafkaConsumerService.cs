using Confluent.Kafka;
using Microservice.C.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.C.Services
{
    public class KafkaConsumerService : IHostedService
    {
        private readonly string topic = "ms-b"; //  from Microservice.B
        public Task StartAsync(CancellationToken cancellationToken)
        {
            //  Can also read from configuration
            var consumerConfiguration = new ConsumerConfig
            {
                GroupId = "default",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var builder = new ConsumerBuilder<Ignore,
                string>(consumerConfiguration).Build())
            {
                builder.Subscribe(topic);
                var cancelToken = new CancellationTokenSource();
                try
                {
                    while (true)
                    {
                        var consumer = builder.Consume(cancelToken.Token);
                        var messageString = consumer.Message.Value;
                        var message = JsonConvert.DeserializeObject<Message>(messageString);

                        Console.WriteLine(consumer.Message.Value);
                    }
                }
                catch (Exception)
                {
                    builder.Close();
                }
            }
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
