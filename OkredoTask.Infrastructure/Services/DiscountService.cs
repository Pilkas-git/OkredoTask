using OkredoTask.Core.Entities;
using OkredoTask.Infrastructure.Data;
using OkredoTask.Infrastructure.Repositories.Interfaces;
using OkredoTask.Infrastructure.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly AppDbContext _dbContext;
        private readonly IDiscountRepository _discountRepository;

        public DiscountService(AppDbContext context,
            IDiscountRepository discountRepository)
        {
            _dbContext = context;
            _discountRepository = discountRepository;
        }

        public async Task<Guid> AddNewDiscountCodeAsync(string discountCode, decimal discountValue, bool isSingleUse, bool isFixedValue,
            DateTime expiresAt)
        {
            var discount = new Discount(discountCode, discountValue, isSingleUse, isFixedValue, expiresAt);
            await _dbContext.AddAsync(discount);

            await _dbContext.SaveChangesAsync();
            return discount.DiscountId;
        }

        public async Task<bool> DeactivateDiscountAsync(Guid discountId)
        {
            var discount = await _discountRepository.GetDiscountByIdAsync(discountId);

            if (discount == null)
            {
                return false;
            }

            discount.Deactivate();

            _dbContext.Entry(discount).CurrentValues.SetValues(discount);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDiscountAsync(Guid discountId, string discountCode, decimal discountValue, bool isSingleUse, bool isFixedValue,
            DateTime expiresAt)
        {
            var discount = await _discountRepository.GetDiscountByIdAsync(discountId);

            if (discount == null)
            {
                return false;
            }

            discount.UpdateDiscountInfo(discountCode, discountValue, isSingleUse, isFixedValue, expiresAt);
            _dbContext.Entry(discount).CurrentValues.SetValues(discount);

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}