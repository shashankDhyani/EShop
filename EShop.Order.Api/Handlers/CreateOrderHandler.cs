using EShop.Infrastructure.Command.Order;
using EShop.Order.DataProvider.Services;
using MassTransit;
using System.Threading.Tasks;

namespace EShop.Order.Api.Handlers
{
    using EShop.Infrastructure.Order;
    public class CreateOrderHandler : IConsumer<CreateOrder>
    {
        IOrderService _service;
        public CreateOrderHandler(IOrderService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<CreateOrder> context)
        {
            var order = new Order {
                Items = context.Message.Items,
                OrderId = context.Message.OrderId,
                UserId = context.Message.UserId
            };

            await _service.CreateOrder(order);
        }
    }
}
