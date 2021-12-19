using System;
using System.ComponentModel.DataAnnotations;

namespace OkredoTask.Web.Models
{
    public class CreateDiscountModel
    {
        [Required]
        public string DiscountCode { get; set; }

        [Required]
        public decimal DiscountValue { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        [Required]
        public bool IsSingleUse { get; set; }

        [Required]
        public bool IsFixedValue { get; set; }
    }
}