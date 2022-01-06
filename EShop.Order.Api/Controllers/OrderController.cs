using System.Threading.Tasks;
using EShop.Order.DataProvider.Services;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Order.Api.Controllers
{
    using EShop.Infrastructure.Order;
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderService _service;
        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder(string orderId)
        {
            var orderDetails = await _service.GetOrder(orderId);
            return Ok(orderDetails);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders(string userId)
        {
            var orderDetails = await _service.GetAllOrders(userId);
            return Ok(orderDetails);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUpdateOrder(Order order)
        {
            var isAdded = await _service.CreateOrder(order);
            return Accepted(isAdded);
        }
    }
}
