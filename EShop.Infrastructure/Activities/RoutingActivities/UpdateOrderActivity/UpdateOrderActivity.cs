using EShop.Infrastructure.Command.Order;
using MassTransit.Courier;
using System;
using System.Threading.Tasks;

namespace EShop.Infrastructure.Activities.RoutingActivities.UpdateOrderActivity
{
    public class UpdateOrderActivity : IActivity<Order.Order, UpdateOrderLog>
    {
        public Task<CompensationResult> Compensate(CompensateContext<UpdateOrderLog> context)
        {
            throw new NotImplementedException();
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<Order.Order> context)
        {
            try
            {
                var endpoint = await context.GetSendEndpoint(new Uri("rabbitmq://localhost/create-order-handler"));
                var createOrder = new CreateOrder
                {
                    Items = context.Arguments.Items,
                    OrderId = context.Arguments.OrderId,
                    UserId = context.Arguments.UserId
                };

                await endpoint.Send(createOrder);

                return context.CompletedWithVariables<UpdateOrderLog>(new
                {
                    context.Arguments.OrderId
                }, new { });
            }
            catch (Exception)
            {
                return context.Faulted();
            }
        }
    }

    public class UpdateOrderLog
    {
        public string OrderId { get; set; }
    }
}
