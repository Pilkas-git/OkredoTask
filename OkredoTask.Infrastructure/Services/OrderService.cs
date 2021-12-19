using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OkredoTask.Core.Enums;
using OkredoTask.Infrastructure.CustomModels;
using OkredoTask.Infrastructure.Data;
using OkredoTask.Infrastructure.Models;
using OkredoTask.Infrastructure.Repositories.Interfaces;
using OkredoTask.Infrastructure.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _dbContext;
        private readonly IOrderRepository _orderRepository;
        private readonly OrderCreatorStrategy _orderCreatorStrategy;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            AppDbContext dbContext,
            IOrderRepository orderRepository,
            ILogger<OrderService> logger,
            IOrderCreatorServiceFactory _orderCreatorServiceFactory,
            IHttpContextAccessor context)
        {
            _dbContext = dbContext;
            _orderCreatorStrategy = context.HttpContext.User.Identity.IsAuthenticated
                ? _orderCreatorServiceFactory.GetOrderService(OrderServiceType.RegularOrder)
                : _orderCreatorServiceFactory.GetOrderService(OrderServiceType.AnonymousOrder);
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderModel orderModel)
        {
            return await _orderCreatorStrategy.CreateOrderAsync(orderModel);
        }

        public async Task<bool> ChangeOrderStatusAsync(Guid orderId, string orderStatus)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);

                if (order == null)
                {
                    _logger.LogError("Invalid orderId for: ChangeOrderStatus");
                    return false;
                }

                //TODO handle order status change effects, In case of cancell update product availability
                order.ChangeStatus(Enum.Parse<OrderStatus>(orderStatus));
                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in OrderService: ChangeOrderStatus");
                return false;
            }
        }
    }
}