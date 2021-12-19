using OkredoTask.Core.Entities;
using System;

namespace OkredoTask.Web.Models
{
    public class CartItemViewModel
    {
        public int Quantity { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }

        public static CartItemViewModel ToModel(CartItem item)
        {
            return new CartItemViewModel()
            {
                Quantity = item.Quantity,
                CartId = item.CartId,
                ProductId = item.ProductId
            };
        }
    }
}