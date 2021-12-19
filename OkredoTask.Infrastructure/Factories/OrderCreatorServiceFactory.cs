using Microsoft.Extensions.DependencyInjection;
using OkredoTask.Core.Enums;
using OkredoTask.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OkredoTask.Infrastructure.Services
{
    public interface IOrderCreatorServiceFactory
    {
        public OrderCreatorStrategy GetOrderService(OrderServiceType orderServiceType);
    }

    public class OrderCreatorServiceFactory : IOrderCreatorServiceFactory
    {
        private readonly IEnumerable<OrderCreatorStrategy> _strategies;

        public OrderCreatorServiceFactory(
            IServiceProvider serviceProvider)
        {
            _strategies = serviceProvider.GetServices<OrderCreatorStrategy>();
        }

        public OrderCreatorStrategy GetOrderService(OrderServiceType orderServiceType)
        {
            return _strategies.First(x => x.GetType().Name.StartsWith(orderServiceType.ToString()));
        }
    }
}