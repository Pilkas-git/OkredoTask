using OkredoTask.Core.Entities;
using OkredoTask.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OkredoTask.Web.Models
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string Comment { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Guid ShippingAddressId { get; set; }
        public Guid? UserId { get; set; }
        public IEnumerable<OrderItemViewModel> OrderItems { get; set; }

        public static OrderViewModel ToModel(Order order)
        {
            return new OrderViewModel
            {
                Id = order.OrderId,
                TotalPrice = order.TotalPrice,
                Comment = order.Comment,
                OrderStatus = order.OrderStatus,
                ShippingAddressId = order.ShippingAddressId,
                UserId = order.UserId,
                OrderItems = OrderItemViewModel.ToModel(order.OrderItems.ToList())
            };
        }

        public static List<OrderViewModel> ToModel(List<Order> orders)
        {
            return orders.Select(order => ToModel(order)).ToList();
        }
    }
}