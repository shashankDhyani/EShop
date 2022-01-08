using EShop.Infrastructure.Inventory;
using EShop.Infrastructure.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Command.Inventory
{
    public class ReleaseProduct
    {
        public List<StockItem> Items { get; set; }
    }
}
