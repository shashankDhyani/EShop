using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Order.Api.Handlers
{
    using EShop.Infrastructure.Activities.RoutingActivities.AllocateProductActivity;
    using EShop.Infrastructure.Activities.RoutingActivities.UpdateCartActivity;
    using EShop.Infrastructure.Activities.RoutingActivities.UpdateOrderActivity;
    using EShop.Infrastructure.Activities.RoutingActivities.WalletActivity;
    using EShop.Infrastructure.Cart;
    using EShop.Infrastructure.Command.Inventory;
    using EShop.Infrastructure.Event.Order;
    using EShop.Infrastructure.Order;
    using MassTransit.Courier;
    using MassTransit.Courier.Contracts;

    public class PlaceOrderHandler : IConsumer<Order>,
        IConsumer<RoutingSlipCompleted>,
        IConsumer<RoutingSlipFaulted>
    {
        private IEndpointNameFormatter _endpointNameFormatter;
        public PlaceOrderHandler(IEndpointNameFormatter endpointNameFormatter)
        {
            _endpointNameFormatter = endpointNameFormatter;
        }
        public async Task Consume(ConsumeContext<Order> context)
        {
            try
            {
                var slip = CreateRoutingSlip(context);
                await context.Execute(slip);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task Consume(ConsumeContext<RoutingSlipCompleted> context)
        {
            try
            {
                var message = context.Message;

                var requestId = message.GetVariable<Guid?>("RequestId");
                var responseAddress = message.GetVariable<Uri>("ResponseAddress");
                var order = message.GetVariable<Order>("PlacedOrder");

                if(requestId.HasValue && responseAddress != null)
                {
                    var endpoint = await context.GetSendEndpoint(responseAddress);
                    await endpoint.Send(new OrderPlaced { OrderId = order.OrderId, RequestId = requestId.Value.ToString(), 
                        Message = "Order Placed." });
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Consume(ConsumeContext<RoutingSlipFaulted> context)
        {
            try
            {
                var message = context.Message;

                var requestId = message.GetVariable<Guid?>("RequestId");
                var responseAddress = message.GetVariable<Uri>("ResponseAddress");
                var order = message.GetVariable<Order>("PlacedOrder");

                if (requestId.HasValue && responseAddress != null)
                {
                    var endpoint = await context.GetSendEndpoint(responseAddress);
                    await endpoint.Send(new OrderPlaced
                    {
                        OrderId = order.OrderId,
                        RequestId = requestId.Value.ToString(),
                        Message = "Order Placement Failed."
                    });
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private RoutingSlip CreateRoutingSlip(ConsumeContext<Order> context)
        {
            Order order = context.Message;
            var routingSlipBuilder = new RoutingSlipBuilder(new Guid(order.OrderId));

            routingSlipBuilder.AddVariable("RequestId", context.RequestId);
            routingSlipBuilder.AddVariable("ResponseAddress", context.ResponseAddress);
            routingSlipBuilder.AddVariable("PlacedOrder", order);

            // Wallet Activity 
            string walletActivityQueueName = _endpointNameFormatter.ExecuteActivity<WalletActivity, TransactMoney>();
            routingSlipBuilder.AddActivity("PROCESS_WALLET", new Uri($"queue:{walletActivityQueueName}"), 
                    new { order.UserId, order.Amount });

            // Allocate Product Activity
            string allocateProductActivityQueueName = _endpointNameFormatter.ExecuteActivity<AllocateProductActivity, AllocateProduct>();
            routingSlipBuilder.AddActivity("ALLOCATE_PRODUCT", new Uri($"queue:{allocateProductActivityQueueName}"), new { });

            // update order details
            string updateOrderActivityQueueName = _endpointNameFormatter.ExecuteActivity<UpdateOrderActivity, Order>();
            routingSlipBuilder.AddActivity("UPDATE_ORDER", new Uri($"queue:{updateOrderActivityQueueName}"), order);

            // update cart 
            string updateCartActivityQueueName = _endpointNameFormatter.ExecuteActivity<UpdateCartActivity, Cart>();
            routingSlipBuilder.AddActivity("UPDATE_CART", new Uri($"queue:{updateCartActivityQueueName}"), 
                new
                {
                   order.UserId
                }
                );

            return routingSlipBuilder.Build();
        }
    }
}
