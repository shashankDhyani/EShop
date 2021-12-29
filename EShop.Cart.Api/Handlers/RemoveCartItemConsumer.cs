using EShop.Cart.DataProvider.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Cart.Api.Handlers
{
    using EShop.Infrastructure.Cart;
    using MassTransit;

    public class RemoveCartItemConsumer : IConsumer<Cart>
    {
        private ICartService _cartService;
        public RemoveCartItemConsumer(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task Consume(ConsumeContext<Cart> context)
        {
            await _cartService.RemoveCart(context.Message.UserId);
            await Task.CompletedTask;
        }
    }
}
