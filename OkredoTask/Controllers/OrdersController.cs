using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using OkredoTask.Core.Constants;
using OkredoTask.Infrastructure.Repositories.Interfaces;
using OkredoTask.Infrastructure.Services.Interfaces;
using OkredoTask.Web.Api;
using OkredoTask.Web.Extensions;
using OkredoTask.Web.Models;
using OkredoTask.Web.Post;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OkredoTask.Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderRepository orderRepository,
            ILogger<OrdersController> logger,
            IOrderService orderService)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _orderService = orderService;
        }

        /// <summary>
        /// Gets order by provided order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderByOrderIdAsync([FromRoute] Guid orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrderByUserAndOrderIdAsync(orderId, User.Identity.GetUserId());

                return order is null ? BadRequest() : Ok(OrderViewModel.ToModel(order));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Order Controller, get order by order id");
                return BadRequest();
            }
        }

        /// <summary>
        /// Gets user order list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOrdersByUserIdAsync()
        {
            try
            {
                var order = await _orderRepository.GetOrdersByUserIdAsync(User.Identity.GetUserId());

                return order is null ? BadRequest() : Ok(OrderViewModel.ToModel(order));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Order Controller, get orders by user id");
                return BadRequest("Unexpected error occured");
            }
        }

        /// <summary>
        /// Creates an order from the provided cartId (via cartCookie header), or user active cart if user is authenticated
        /// Authenticated user can pass addressId to use already created address.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderModel body)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var createOrder = await _orderService.CreateOrderAsync(body.ToCreateOrderModelWithUserId(User.Identity.GetUserId()));
                    return createOrder.Success ? Ok() : BadRequest(createOrder.Error);
                }

                if (!Request.Headers.TryGetValue(CookieConstants.CartCookie, out StringValues headerValues))
                {
                    return BadRequest("No cartId cookie found");
                }

                var cartCookie = headerValues.FirstOrDefault();

                var result = await _orderService.CreateOrderAsync(body.ToCreateOrderModelWithCartId(Guid.Parse(cartCookie)));
                return result.Success ? Ok() : BadRequest(result.Error);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occurred in Order Controller, create order");
                return BadRequest("Unexpected error occured");
            }
        }
    }
}