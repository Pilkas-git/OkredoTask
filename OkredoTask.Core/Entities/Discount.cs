using System;

namespace OkredoTask.Core.Entities
{
    public class Discount : PersistentEntityBase
    {
        public Guid DiscountId { get; private set; }
        public string DiscountCode { get; private set; }
        public decimal DiscountValue { get; private set; }
        public bool IsSingleUse { get; private set; }
        public bool IsFixedValue { get; private set; }
        public bool IsValid { get; private set; }
        public DateTime Expires { get; private set; }

        private Discount()
        {
        }

        public Discount(string discountCode, decimal discountValue, bool isSingleUse, bool isFixedValue, DateTime expiresAt)
        {
            DiscountCode = discountCode;
            DiscountValue = discountValue;
            IsSingleUse = isSingleUse;
            IsFixedValue = isFixedValue;
            IsValid = true;
            Expires = expiresAt;
        }

        public void Deactivate() => IsValid = false;

        public void UpdateDiscountInfo(string discountCode, decimal discountValue, bool isSingleUse, bool isFixedValue, DateTime expiresAt)
        {
            DiscountCode = discountCode;
            DiscountValue = discountValue;
            IsSingleUse = isSingleUse;
            IsFixedValue = isFixedValue;
            Expires = expiresAt;
        }
    }
}