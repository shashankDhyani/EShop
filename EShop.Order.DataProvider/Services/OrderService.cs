using EShop.Order.DataProvider.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Order.DataProvider.Services
{
    public class OrderService : IOrderService
    {
        private IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<bool> CreateOrder(Infrastructure.Order.Order order)
        {
            return await _orderRepository.CreateOrder(order);
        }

        public async Task<List<Infrastructure.Order.Order>> GetAllOrders(string UserId)
        {
            return await _orderRepository.GetAllOrders(UserId);
        }

        public async Task<Infrastructure.Order.Order> GetOrder(string OrderId)
        {
            return await _orderRepository.GetOrder(OrderId);

        }
    }
}
