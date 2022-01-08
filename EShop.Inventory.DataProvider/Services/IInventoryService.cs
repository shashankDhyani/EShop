using EShop.Infrastructure.Command.Inventory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Inventory.DataProvider.Services
{
    public interface IInventoryService
    {
        Task<bool> AddStocks(AllocateProduct stock);
        Task<bool> ReleaseStocks(ReleaseProduct stock);
    }
}
