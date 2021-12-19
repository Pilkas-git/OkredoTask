using OkredoTask.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OkredoTask.Web.Models
{
    public class CartViewModel
    {
        public Guid CartId { get; set; }
        public decimal Price { get; set; }
        public Guid? UserId { get; set; }
        public string DiscountCode { get; set; }
        public List<CartItemViewModel> CartItems { get; set; }

        public static CartViewModel ToModel(Cart cart)
        {
            return new CartViewModel
            {
                CartId = cart.CartId,
                Price = cart.Price,
                UserId = cart.UserId,
                DiscountCode = cart.Discount?.DiscountCode,
                CartItems = cart.CartItems.Select(CartItemViewModel.ToModel).ToList()
            };
        }
    }
}