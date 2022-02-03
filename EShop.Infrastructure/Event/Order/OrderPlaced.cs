using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Event.Order
{
    public class OrderPlaced
    {
        public string OrderId { get; set; }
        public string RequestId { get; set; }
        public string Message { get; set; }
    }
}
