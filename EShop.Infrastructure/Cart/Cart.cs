using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Cart
{
    public class Cart
    {
        public string CartId { get; set; }
        public List<CartItem> Items { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
    }
}
