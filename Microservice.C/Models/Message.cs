using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.C.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string MessageTitle { get; set; }
        public string MessageBody { get; set; }
        public DateTime CreatedOn { get; set; }

    }

    public class RabbitMQConfig
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
    }
}
