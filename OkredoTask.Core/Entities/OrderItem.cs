using System;

namespace OkredoTask.Core.Entities
{
    public class OrderItem : PersistentEntityBase
    {
        public int Quantity { get; private set; }
        public Guid OrderId { get; private set; }
        public Order Order { get; private set; }
        public Guid ProductId { get; private set; }
        public Product Product { get; private set; }

        private OrderItem()
        {
        }

        public OrderItem(Order order, Product product, int quantity)
        {
            Order = order;
            Product = product;
            Quantity = quantity;
        }
    }
}