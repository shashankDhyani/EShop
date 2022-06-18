using EShop.Infrastructure.Command.Product;
using EShop.Infrastructure.Common;
using EShop.Infrastructure.Event.Product;
using System.Threading.Tasks;

namespace Eshop.Product.DataProvider.Repository
{
    public interface IProductRepository : IIdempotent
    {
        Task<ProductCreated> AddProduct(CreateProduct product);
        Task<ProductCreated> GetProduct(string ProductId);
    }
}
