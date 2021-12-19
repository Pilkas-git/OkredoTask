using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OkredoTask.Core.Constants;
using OkredoTask.Infrastructure.Repositories.Interfaces;
using OkredoTask.Infrastructure.Services.Interfaces;
using OkredoTask.Web.Api;
using OkredoTask.Web.Models;
using OkredoTask.Web.Patch;
using System;
using System.Threading.Tasks;

namespace OkredoTask.Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = RoleConstants.Admin)]
    [Route("Admin/Orders")]
    public class OrdersManagement : BaseApiController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersManagement> _logger;

        public OrdersManagement(IOrderRepository orderRepository,
            ILogger<OrdersManagement> logger,
            IOrderService orderService)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _orderService = orderService;
        }

        /// <summary>
        /// Gets all orders. If userId is specified returns all user orders.
        /// </summary>
        /// <returns>A list of orders</returns>
        [Authorize(Roles = RoleConstants.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] Guid? userId)
        {
            try
            {
                if (userId.HasValue)
                {
                    var userOrders = await _orderRepository.GetOrdersByUserIdAsync(userId.Value);
                    return userOrders is null ? BadRequest() : Ok(OrderViewModel.ToModel(userOrders));
                }

                var orders = await _orderRepository.GetAllOrdersAsync(pageNumber, pageSize);
                return orders is null ? BadRequest() : Ok(OrderViewModel.ToModel(orders));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Order Controller, get order by order id");
                return BadRequest();
            }
        }

        /// <summary>
        /// Updates Order status
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderStatusPatch">Body, containing new order Status</param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPatch("{orderId}")]
        public async Task<IActionResult> UpdateStatusAsync([FromRoute] Guid orderId, [FromBody] OrderStatusPatch orderStatusPatch)
        {
            try
            {
                var result = await _orderService.ChangeOrderStatusAsync(orderId, orderStatusPatch.OrderStatus);

                return result ? Ok() : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occurred in Order Controller, create order");
                return BadRequest("Unexpected error occured");
            }
        }
    }
}