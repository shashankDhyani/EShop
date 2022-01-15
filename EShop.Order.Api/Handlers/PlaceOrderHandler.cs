using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Order.Api.Handlers
{
    using EShop.Infrastructure.Order;
    using MassTransit.Courier;
    using MassTransit.Courier.Contracts;

    public class PlaceOrderHandler : IConsumer<Order>
    {
        public Task Consume(ConsumeContext<Order> context)
        {
            throw new NotImplementedException();
        }

        private RoutingSlip CreateRoutingSlip(Order order, ConsumeContext<Order> context)
        {
            var routingSlipBuilder = new RoutingSlipBuilder(new Guid(order.OrderId));

            routingSlipBuilder.AddVariable("RequestId", context.RequestId);
            routingSlipBuilder.AddVariable("ResponseAddress", context.ResponseAddress);
            routingSlipBuilder.AddVariable("PlacedOrder", order);

            // Wallet Activity 
            routingSlipBuilder.AddActivity("PROCESS_WALLET", new Uri("queue://wallet_execute"), 
                    new { order.UserId, order.Amount });

            // Allocate Product Activity
            routingSlipBuilder.AddActivity("ALLOCATE_PRODUCT", new Uri("queue://allocate-product_execute"), new { });

            // update order details
            routingSlipBuilder.AddActivity("UPDATE_ORDER", new Uri("queue://update-order_execute"), order);

            // update cart 
            routingSlipBuilder.AddActivity("UPDATE_CART", new Uri("queue://update_cart_execute"), 
                new
                {
                   order.UserId
                }
                );

            return routingSlipBuilder.Build();
        }
    }
}
