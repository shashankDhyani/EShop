using System.Threading.Tasks;
using Eshop.Product.DataProvider.Service;
using EShop.Infrastructure.Event.Product;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Product.Query.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ProductCreated> GetProduct(string productId)
        {
            var product = await _productService.GetProduct(productId);
            return product;
        }
    }
}