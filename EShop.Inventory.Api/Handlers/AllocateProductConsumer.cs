using EShop.Infrastructure.Command.Inventory;
using EShop.Inventory.DataProvider.Services;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Inventory.Api.Handlers
{
    public class AllocateProductConsumer : IConsumer<AllocateProduct>
    {
        private IInventoryService _inventoryService;
        public AllocateProductConsumer(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }
        public async Task Consume(ConsumeContext<AllocateProduct> context)
        {
            await _inventoryService.AddStocks(context.Message);
            await Task.CompletedTask;
        }
    }
}
