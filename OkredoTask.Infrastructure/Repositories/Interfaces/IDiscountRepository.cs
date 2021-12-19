using OkredoTask.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Repositories.Interfaces
{
    public interface IDiscountRepository
    {
        public Task<Discount> GetDiscountByIdAsync(Guid discountId);

        public Task<Discount> GetDiscountByCodeAsync(string discountCode);

        public Task<List<Discount>> GetDiscountListAsync(int pageNumber, int pageSize);
    }
}