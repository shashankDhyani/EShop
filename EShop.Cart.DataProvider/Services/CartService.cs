using EShop.Cart.DataProvider.Repository;
using System.Threading.Tasks;

namespace EShop.Cart.DataProvider.Services
{
    public class CartService : ICartService
    {
        private ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<bool> AddCart(Infrastructure.Cart.Cart cart)
        {
            return await _cartRepository.AddCart(cart);
        }

        public async Task<Infrastructure.Cart.Cart> GetCart(string UserId)
        {
            return await _cartRepository.GetCart(UserId);
        }

        public async Task<bool> RemoveCart(string UserId)
        {
            return await _cartRepository.RemoveCart(UserId);
        }
    }
}
