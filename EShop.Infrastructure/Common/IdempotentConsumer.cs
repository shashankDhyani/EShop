using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Common
{
    public class IdempotentConsumer
    {
        public Guid? MessageId { get; set; }
        public string ConsumerName { get; set; }
    }
}
