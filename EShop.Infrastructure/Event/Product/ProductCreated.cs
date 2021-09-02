using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Event.Product
{
    public class ProductCreated
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
