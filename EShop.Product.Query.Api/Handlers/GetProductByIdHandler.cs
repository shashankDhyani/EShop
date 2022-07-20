using Eshop.Product.DataProvider.Service;
using EShop.Infrastructure.Event.Product;
using EShop.Infrastructure.Query.Product;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Product.Query.Api.Handlers
{
    [Obsolete("This consumer will no longer be called for request/response")]
    public class GetProductByIdHandler : IConsumer<GetProductById>
    {
        private IProductService _service;
        public static int EXCEPTION_COUNT = 0;
        public GetProductByIdHandler(IProductService service)
        {
            _service = service;
        }
        
        public async Task Consume(ConsumeContext<GetProductById> context)
        {
            var product = await _service?.GetProduct(context.Message.ProductId);
            await context.RespondAsync<ProductCreated>(product);
        }
    }
}
