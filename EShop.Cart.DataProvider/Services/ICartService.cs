using System.Threading.Tasks;

namespace EShop.Cart.DataProvider.Services
{
    using EShop.Infrastructure.Cart;
    public interface ICartService
    {
        Task<bool> AddCart(Cart cart);
        Task<Cart> GetCart(string UserId);
        Task<bool> RemoveCart(string UserId);
    }
}
