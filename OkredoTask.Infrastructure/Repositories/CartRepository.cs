using Microsoft.EntityFrameworkCore;
using OkredoTask.Core.Entities;
using OkredoTask.Infrastructure.Data;
using OkredoTask.Infrastructure.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _dbContext;

        public CartRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Cart> GetCartByIdAsync(Guid id)
        {
            return await _dbContext.Carts.FirstOrDefaultAsync(c => c.CartId == id);
        }

        public async Task<Cart> GetCartWithItemsByIdAsync(Guid id)
        {
            return await _dbContext.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(p => p.Product)
                .Include(c => c.Discount)
                .FirstOrDefaultAsync(c => c.CartId == id);
        }

        public async Task<Cart> GetCartByUserIdAsync(Guid userId)
        {
            return await _dbContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart> GetCartWithItemsByUserIdAsync(Guid userId)
        {
            return await _dbContext.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(p => p.Product)
                .Include(c => c.Discount)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}