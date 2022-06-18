using Eshop.Product.DataProvider.Service;
using EShop.Infrastructure.Command.Product;
using MassTransit;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace EShop.Product.Api.Handlers
{
    public class CreateProductHandler : IConsumer<CreateProduct>
    {
        private IProductService _service;
        private IMongoClient _client;
        public CreateProductHandler(IProductService productService, IMongoClient client)
        {
            _service = productService;
            _client = client;
        }
        public async Task Consume(ConsumeContext<CreateProduct> context)
        {
            using(var session = await _client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();
                    var isNew = await _service.IsNewMessage(context.MessageId);

                    if (isNew)
                    {
                        await _service.AddProduct(context.Message);
                        await _service.AddMessage(nameof(CreateProductHandler), context.MessageId);
                        await Task.CompletedTask;
                    }
                    await session.CommitTransactionAsync();
                }
                catch (System.Exception)
                {
                    await session.AbortTransactionAsync();
                }
            }
        }
    }
}
