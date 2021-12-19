using OkredoTask.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OkredoTask.Core.Entities
{
    public class Cart : PersistentEntityBase
    {
        public Guid CartId { get; private set; }
        public decimal Price { get; private set; }
        public Guid? UserId { get; private set; }
        public User User { get; private set; }
        public Guid? DiscountId { get; private set; }
        public Discount Discount { get; private set; }
        private readonly List<CartItem> _cartItems = new();
        public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

        public Cart()
        {
            Price = 0;
            CartId = Guid.NewGuid();
        }

        public void SetUser(Guid id)
        {
            UserId = id;
        }

        public void UpdateCartPrice()
        {
            decimal price = 0;

            foreach (var item in _cartItems)
            {
                price += item.Product.Price * item.Quantity;
            }

            if (Discount != null)
            {
                price = Discount.IsFixedValue ? price - Discount.DiscountValue : price - price * Discount.DiscountValue;

                if (price < 0)
                {
                    price = 0;
                }
            }

            SetPrice(price);
        }

        public void AddCartItem(Product product, int quantity)
        {
            _cartItems.Add(new CartItem(this, product, quantity));
            UpdateCartPrice();
        }

        public void RemoveCartItem(Guid productId)
        {
            var cartItemToRemove = _cartItems.FirstOrDefault(x => x.ProductId == productId);

            if (cartItemToRemove is null)
            {
                throw new DomainException("Failed to remove cart item. Item not found");
            }
            _cartItems.Remove(_cartItems.FirstOrDefault(x => x.ProductId == productId));

            UpdateCartPrice();
        }

        public void ApplyDiscountCode(Discount discount)
        {
            Discount = discount;
            UpdateCartPrice();
        }

        private void SetPrice(decimal price)
        {
            Price = price;
        }
    }
}