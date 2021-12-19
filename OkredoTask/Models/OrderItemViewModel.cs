using OkredoTask.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OkredoTask.Web.Models
{
    public class OrderItemViewModel
    {
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }

        public static OrderItemViewModel ToModel(OrderItem orderItem)
        {
            return new OrderItemViewModel()
            {
                Quantity = orderItem.Quantity,
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId
            };
        }

        public static List<OrderItemViewModel> ToModel(List<OrderItem> orderItems)
        {
            return orderItems.Select(orderItem => ToModel(orderItem)).ToList();
        }
    }
}