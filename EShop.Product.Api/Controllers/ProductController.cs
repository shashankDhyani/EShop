using System;
using System.Threading.Tasks;
using Eshop.Product.DataProvider.Service;
using EShop.Infrastructure.Command.Product;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Product.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public ProductController(IProductService service)
        {
            _service = service;
        }

        private IProductService _service { get; }

        public async Task<IActionResult> Get(string productId)
        {
            var product = await _service.GetProduct(productId);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateProduct product)
        {
            var addedProduct = await _service.AddProduct(product);
            return Ok(addedProduct);
        }
    }
}