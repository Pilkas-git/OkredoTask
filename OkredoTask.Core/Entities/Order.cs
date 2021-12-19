using OkredoTask.Core.Enums;
using OkredoTask.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OkredoTask.Core.Entities
{
    public class Order : PersistentEntityBase
    {
        public Guid OrderId { get; private set; }
        public decimal TotalPrice { get; private set; }
        public string Comment { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public Guid ShippingAddressId { get; private set; }
        public Address ShippingAddress { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public Guid? DiscountId { get; private set; }
        public Discount Discount { get; private set; }

        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        private Order()
        {
        }

        public Order(Guid userId, Guid addressId, string comment, decimal totalPrice)
        {
            UserId = userId;
            ShippingAddressId = addressId;
            Comment = comment;
            TotalPrice = totalPrice;
            OrderStatus = OrderStatus.Confirmed;
        }

        public void ChangeStatus(OrderStatus status)
        {
            OrderStatus = status;
        }

        public void AddOrderItems(List<OrderItem> orderItems)
        {
            foreach (var orderItem in orderItems)
            {
                _orderItems.Add(orderItem);
            }
        }

        public void AddUsedDiscount(Discount discount)
        {
            Discount = discount;
        }

        public void RemoveOrderItem(Guid productId)
        {
            var orderItemToRemove = _orderItems.FirstOrDefault(x => x.ProductId == productId);

            if (orderItemToRemove is null)
            {
                throw new DomainException("Failed to remove order item. Item not found");
            }

            _orderItems.Remove(_orderItems.FirstOrDefault(x => x.ProductId == productId));
        }
    }
}