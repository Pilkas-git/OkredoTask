using OkredoTask.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OkredoTask.Web.Models
{
    public class DiscountViewModel
    {
        public Guid DiscountId { get; set; }
        public string DiscountCode { get; set; }
        public decimal DiscountValue { get; set; }
        public bool IsSingleUse { get; set; }
        public bool IsValid { get; set; }
        public DateTime Expires { get; set; }
        public bool IsFixedValue { get; set; }

        public static DiscountViewModel ToModel(Discount discount)
        {
            return new DiscountViewModel
            {
                DiscountId = discount.DiscountId,
                DiscountCode = discount.DiscountCode,
                DiscountValue = discount.DiscountValue,
                IsSingleUse = discount.IsSingleUse,
                IsValid = discount.IsValid,
                Expires = discount.Expires,
                IsFixedValue = discount.IsFixedValue
            };
        }

        public static List<DiscountViewModel> ToModel(List<Discount> discounts)
        {
            return discounts.Select(d => ToModel(d)).ToList();
        }
    }
}