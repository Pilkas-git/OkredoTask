using Microsoft.EntityFrameworkCore;
using OkredoTask.Core.Entities;
using OkredoTask.Infrastructure.Data;
using OkredoTask.Infrastructure.Extensions;
using OkredoTask.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _dbContext.Orders
                .Include(c => c.OrderItems)
                .FirstOrDefaultAsync(c => c.OrderId == orderId);
        }

        public async Task<Order> GetOrderByUserAndOrderIdAsync(Guid orderId, Guid userId)
        {
            return await _dbContext.Orders
                .Include(c => c.OrderItems)
                .FirstOrDefaultAsync(c => c.OrderId == orderId && c.UserId == userId);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _dbContext.Orders
                .Include(c => c.OrderItems)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllOrdersAsync(int pageNumber, int pageSize)
        {
            return await _dbContext.Orders
                .AsNoTracking()
                .Include(c => c.OrderItems)
                .Page(pageNumber, pageSize)
                .ToListAsync();
        }
    }
}