using Eshop.Product.DataProvider.Service;
using EShop.Infrastructure.Command.Product;
using MassTransit;
using System.Threading.Tasks;

namespace EShop.Product.Api.Handlers
{
    public class CreateProductHandler : IConsumer<CreateProduct>
    {
        private IProductService _service;
        public CreateProductHandler(IProductService productService)
        {
            _service = productService;
        }
        public async Task Consume(ConsumeContext<CreateProduct> context)
        {
            await _service.AddProduct(context.Message);
            await Task.CompletedTask;
        }
    }
}
