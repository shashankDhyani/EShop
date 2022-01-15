using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Infrastructure.Activities.RoutingActivities.UpdateCartActivity
{
    public class UpdateCartActivity : IExecuteActivity<Cart.Cart>
    {
        public async Task<ExecutionResult> Execute(ExecuteContext<Cart.Cart> context)
        {
            try
            {
                var endpoint = await context.GetSendEndpoint(new Uri("rabbitmq://localhost/remove_cart"));
                await endpoint.Send(context.Arguments);

                return context.Completed();
            }
            catch (Exception)
            {
                throw new Exception("Error while updating cart, batch file should take care of this.");
            }
        }
    }
}
