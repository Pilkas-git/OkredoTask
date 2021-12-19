using System;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Services.Interfaces
{
    public interface IDiscountService
    {
        public Task<Guid> AddNewDiscountCodeAsync(string discountCode, decimal discountValue, bool isSingleUse, bool isFixedValue, DateTime expiresAt);

        public Task<bool> DeactivateDiscountAsync(Guid discountId);

        public Task<bool> UpdateDiscountAsync(Guid discountId, string discountCode, decimal discountValue, bool isSingleUse, bool isFixedValue,
            DateTime expiresAt);
    }
}