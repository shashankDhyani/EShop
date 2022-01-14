using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShop.Infrastructure.Command.Inventory;
using EShop.Inventory.DataProvider.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EShop.Inventory.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StockController : ControllerBase
    {
        private IInventoryService _inventoryService;
        public StockController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPost]
        public async Task<IActionResult> AddStocks(AllocateProduct stock)
        {
            var isAdded = await _inventoryService.AddStocks(stock);
            return Accepted(isAdded);
        }

        [HttpPost]
        public async Task<IActionResult> ReleaseStocks(ReleaseProduct stock)
        {
            var isReleased = await _inventoryService.ReleaseStocks(stock);
            return Accepted(isReleased);
        }
    }
}
