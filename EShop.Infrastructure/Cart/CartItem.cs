using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Cart
{
    public class CartItem
    {
        public string  ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
