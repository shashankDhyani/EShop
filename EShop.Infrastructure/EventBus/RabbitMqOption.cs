using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.EventBus
{
    public class RabbitMqOption
    {
        public string ConnectionString { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
