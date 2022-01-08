using EShop.Infrastructure.Command.Inventory;
using EShop.Inventory.DataProvider.Repository;
using System.Threading.Tasks;

namespace EShop.Inventory.DataProvider.Services
{
    public class InventoryService : IInventoryService
    {
        private IInventoryRepository _inventoryRepository;
        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }
        public async Task<bool> AddStocks(AllocateProduct stock)
        {
            return await _inventoryRepository.AddStocks(stock);
        }

        public async Task<bool> ReleaseStocks(ReleaseProduct stock)
        {
            return await _inventoryRepository.ReleaseStocks(stock);
        }
    }
}
