using Eshop.Product.DataProvider.Repository;
using EShop.Infrastructure.Command.Product;
using EShop.Infrastructure.Event.Product;
using System.Threading.Tasks;

namespace Eshop.Product.DataProvider.Service
{
    public class ProductService : IProductService
    {
        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        private IProductRepository _repository;

        public async Task<ProductCreated> AddProduct(CreateProduct product)
        {
            return await _repository.AddProduct(product);
        }

        public async Task<ProductCreated> GetProduct(string ProductId)
        {
            var product = await _repository.GetProduct(ProductId);
            return product;
        }
    }
}
