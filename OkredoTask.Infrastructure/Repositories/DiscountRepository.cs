using Microsoft.EntityFrameworkCore;
using OkredoTask.Core.Entities;
using OkredoTask.Infrastructure.Data;
using OkredoTask.Infrastructure.Extensions;
using OkredoTask.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly AppDbContext _dbContext;

        public DiscountRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Discount> GetDiscountByCodeAsync(string discountCode)
        {
            return await _dbContext.Discounts.FirstOrDefaultAsync(d => d.DiscountCode == discountCode);
        }

        public async Task<Discount> GetDiscountByIdAsync(Guid discountId)
        {
            return await _dbContext.Discounts.FirstOrDefaultAsync(d => d.DiscountId == discountId);
        }

        public async Task<List<Discount>> GetDiscountListAsync(int pageNumber, int pageSize)
        {
            return await _dbContext.Discounts.Page(pageNumber, pageSize).AsNoTracking().ToListAsync();
        }
    }
}