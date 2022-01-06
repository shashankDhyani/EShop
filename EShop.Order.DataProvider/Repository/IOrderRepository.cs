using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Order.DataProvider.Repository
{
    using EShop.Infrastructure.Order;
    public interface IOrderRepository
    {
        Task<Order> GetOrder(string OrderId);
        Task<List<Order>> GetAllOrders(string UserId);

        Task<bool> CreateOrder(Order order);
    }
}
