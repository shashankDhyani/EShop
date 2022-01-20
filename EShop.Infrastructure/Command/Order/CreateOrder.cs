using EShop.Infrastructure.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EShop.Infrastructure.Command.Order
{
    public class CreateOrder
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal Amount
        {
            get
            {
                return Items.Sum(item => item.Price * item.Quantity);
            }
        }
    }
}
