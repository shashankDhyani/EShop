using EShop.Infrastructure.Command.Product;
using EShop.Infrastructure.Common;
using EShop.Infrastructure.Event.Product;
using System.Threading.Tasks;

namespace Eshop.Product.DataProvider.Service
{
    public interface IProductService : IIdempotent
    {
        Task<ProductCreated> GetProduct(string ProductId);
        Task<ProductCreated> AddProduct(CreateProduct product);
    }
}
