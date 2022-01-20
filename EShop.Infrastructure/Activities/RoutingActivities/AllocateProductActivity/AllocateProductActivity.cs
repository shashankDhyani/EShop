using EShop.Infrastructure.Command.Inventory;
using MassTransit.Courier;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Infrastructure.Activities.RoutingActivities.AllocateProductActivity
{
    public class AllocateProductActivity : IActivity<AllocateProduct, OrderLog>
    {
        public Task<CompensationResult> Compensate(CompensateContext<OrderLog> context)
        {
            throw new NotImplementedException();
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<AllocateProduct> context)
        {
            try
            {
            var endpoint = await context.GetSendEndpoint(new Uri("rabbitmq://localhost/release_product"));
            var order = JsonConvert.DeserializeObject<ReleaseProduct>(context.Message.Variables["PlacedOrder"].ToString());

            await endpoint.Send(order);

            return context.CompletedWithVariables<ReleaseProduct>(order, new { });
            }
            catch (Exception)
            {
                return context.Faulted();
            }
        }
    }

    public class OrderLog
    {
        public Order.Order Order { get; set; }
        public string Message { get; set; }
    }
}
