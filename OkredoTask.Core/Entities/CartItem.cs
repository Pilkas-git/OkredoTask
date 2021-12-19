using System;
using System.Collections.Generic;
using System.Linq;

namespace OkredoTask.Core.Entities
{
    public class CartItem : PersistentEntityBase
    {
        public Guid CartId { get; private set; }
        public Cart Cart { get; private set; }
        public Guid ProductId { get; private set; }
        public Product Product { get; private set; }
        public int Quantity { get; private set; }

        private CartItem()
        {
        }

        public CartItem(Cart cart, Product product, int quantity)
        {
            Cart = cart;
            Product = product;
            Quantity = quantity;
        }

        public void SetQuantity(int quantity) => Quantity = quantity;

        public static List<OrderItem> ToOrderItems(List<CartItem> cartItems, Order order)
        {
            return cartItems.Select(i => new OrderItem(order, i.Product, i.Quantity)).ToList();
        }
    }
}