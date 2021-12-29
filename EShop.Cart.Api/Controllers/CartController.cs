using EShop.Cart.DataProvider.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EShop.Cart.Api.Controllers
{
    using EShop.Infrastructure.Cart;
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private ICartService _service;
        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string UserId)
        {
            var cart = await _service.GetCart(UserId);
            return Ok(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Cart cart)
        {
            var isAdded = await _service.AddCart(cart);
            return Accepted(isAdded);
        }
    }
}
