using EShop.Infrastructure.Command.Inventory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Inventory.DataProvider.Repository
{
    public interface IInventoryRepository
    {
        Task<bool> AddStocks(AllocateProduct stock);
        Task<bool> ReleaseStocks(ReleaseProduct stock);
    }
}
