using System;
using System.ComponentModel.DataAnnotations;

namespace OkredoTask.Web.Models
{
    public class CartItemPatchModel
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}