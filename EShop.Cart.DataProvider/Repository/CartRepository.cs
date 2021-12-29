using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace EShop.Cart.DataProvider.Repository
{
    public class CartRepository : ICartRepository
    {
        private IDistributedCache _distributedCache;
        public CartRepository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<bool> AddCart(Infrastructure.Cart.Cart cart)
        {
            try
            {
                await _distributedCache.SetStringAsync(cart.UserId, JsonConvert.SerializeObject(cart));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Infrastructure.Cart.Cart> GetCart(string UserId)
        {
            var existingCart = await _distributedCache.GetStringAsync(UserId);

            if (string.IsNullOrEmpty(existingCart))
                return new Infrastructure.Cart.Cart();

            return JsonConvert.DeserializeObject<Infrastructure.Cart.Cart>(existingCart);
        }

        public async Task<bool> RemoveCart(string UserId)
        {
            await _distributedCache.RemoveAsync(UserId);
            return true;
        }
    }
}
