using OkredoTask.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace OkredoTask.Web.Models
{
    public class CreateProductModel
    {
        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [StringLength(512)]
        public string Description { get; set; }

        [Required]
        public ProductType ProductType { get; set; }

        [Required]
        public Guid SupplierId { get; set; }

        public int AvailabilityCount { get; set; }
    }
}