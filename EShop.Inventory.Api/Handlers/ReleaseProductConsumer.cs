using EShop.Infrastructure.Command.Inventory;
using EShop.Inventory.DataProvider.Services;
using MassTransit;
using System.Threading.Tasks;

namespace EShop.Inventory.Api.Handlers
{
    public class ReleaseProductConsumer : IConsumer<ReleaseProduct>
    {
        private IInventoryService _inventoryService;
        public ReleaseProductConsumer(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }
        public async Task Consume(ConsumeContext<ReleaseProduct> context)
        {
            await _inventoryService.ReleaseStocks(context.Message);
            await Task.CompletedTask;
        }
    }
}
