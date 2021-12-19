using OkredoTask.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task<Order> GetOrderByIdAsync(Guid orderId);

        public Task<Order> GetOrderByUserAndOrderIdAsync(Guid orderId, Guid userId);

        public Task<List<Order>> GetOrdersByUserIdAsync(Guid userId);

        public Task<List<Order>> GetAllOrdersAsync(int pageNumber, int pageSize);
    }
}