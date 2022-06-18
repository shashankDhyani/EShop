using EShop.Infrastructure.Command.Product;
using EShop.Infrastructure.Common;
using EShop.Infrastructure.Event.Product;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Threading.Tasks;

namespace Eshop.Product.DataProvider.Repository
{
    public class ProductRepository : IProductRepository
    {
        private IMongoDatabase _database;
        private IMongoCollection<CreateProduct> _collection;
        private IMongoCollection<IdempotentConsumer> _messageCollection;

        public ProductRepository(IMongoDatabase database)
        {
            _database = database;
            _collection = database.GetCollection<CreateProduct>("product", null);
            _messageCollection = database.GetCollection<IdempotentConsumer>("messageLog", null);
        }


        public async Task<ProductCreated> AddProduct(CreateProduct product)
        {
            await _collection.InsertOneAsync(product);
            return new ProductCreated { ProductId = product.ProductId, ProductName = product.ProductName, CreatedAt = DateTime.UtcNow };
        }

        public async Task<ProductCreated> GetProduct(string ProductId)
        {
            var product = new CreateProduct();
            product = await _collection.AsQueryable().FirstOrDefaultAsync(x => x.ProductId == ProductId);
            return new ProductCreated() { ProductId = product.ProductId, ProductName = product.ProductName };

        }


        public async Task AddMessage(string ConsumerName, Guid? MessageId)
        {
            var entry = new IdempotentConsumer()
            {
                ConsumerName = ConsumerName,
                MessageId = MessageId
            };

            await _messageCollection.InsertOneAsync(entry);
        }

        public async Task<bool> IsNewMessage(Guid? MessageId)
        {
            var loggedMessage = await _messageCollection.AsQueryable().FirstOrDefaultAsync(message => message.MessageId == MessageId);

            return loggedMessage is null;
        }
    }
}
