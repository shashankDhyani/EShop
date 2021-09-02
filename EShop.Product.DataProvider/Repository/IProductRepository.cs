using EShop.Infrastructure.Command.Product;
using EShop.Infrastructure.Event.Product;
using System.Threading.Tasks;

namespace Eshop.Product.DataProvider.Repository
{
    public interface IProductRepository
    {
        Task<ProductCreated> AddProduct(CreateProduct product);
        Task<ProductCreated> GetProduct(string ProductId);
    }
}
