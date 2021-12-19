using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using OkredoTask.Core.Constants;
using OkredoTask.Infrastructure.Repositories.Interfaces;
using OkredoTask.Infrastructure.Services.Interfaces;
using OkredoTask.Web.Api;
using OkredoTask.Web.Extensions;
using OkredoTask.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OkredoTask.Web.Controllers
{
    public class ShoppingCartController : BaseApiController
    {
        private readonly ILogger<ShoppingCartController> _logger;
        private readonly ICartRepository _shoppingCartRepository;
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(ILogger<ShoppingCartController> logger,
            ICartRepository shoppingCartRepository,
            IShoppingCartService shoppingCartService)
        {
            _logger = logger;
            _shoppingCartRepository = shoppingCartRepository;
            _shoppingCartService = shoppingCartService;
        }

        /// <summary>
        /// Gets active shopping cart, if client has no cart, it creates and assigns a new cart.
        /// If user is not logged in it provides cartId, which can be used in header
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCartAsync()
        {
            try
            {
                // If user is authenticated
                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    var userCart = await _shoppingCartService.GetUserCartAsync(User.Identity.GetUserId());

                    return Ok(CartViewModel.ToModel(userCart));
                }

                // If user has "cartCookie" header value
                if (Request.Headers.TryGetValue(CookieConstants.CartCookie, out StringValues headerValues))
                {
                    var cartCookie = headerValues.FirstOrDefault();
                    var localCart = await _shoppingCartRepository.GetCartWithItemsByIdAsync(Guid.Parse(cartCookie));

                    return localCart is null ? BadRequest() : Ok(CartViewModel.ToModel(localCart));
                }

                var cart = await _shoppingCartService.CreateCartAsync();

                return Ok(CartViewModel.ToModel(cart));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occurred in Shopping Cart Controller, GET shopping cart");
                return BadRequest();
            }
        }

        /// <summary>
        /// Adds specified item to active cart
        /// User must have an active cart. Request must contain cartCookie header if user is not logged in.
        /// </summary>
        /// <param name="cartItemPatchModel"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> AddItemToCartAsync([FromBody] CartItemPatchModel cartItemPatchModel)
        {
            try
            {
                var cartId = Request.Headers.TryGetValue(CookieConstants.CartCookie, out StringValues headerValues)
                                 ? Guid.Parse(headerValues.FirstOrDefault())
                                 : (await _shoppingCartRepository.GetCartByUserIdAsync(User.Identity.GetUserId()))?.CartId;

                if (cartId == null)
                {
                    return BadRequest("User has no active cart");
                }

                var result = await _shoppingCartService.AddItemToCartAsync(cartId.Value, cartItemPatchModel.ProductId,
                    cartItemPatchModel.Quantity);

                return result is false ? BadRequest() : Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occurred in Shopping Cart Controller, Add shopping cart item");
                return BadRequest();
            }
        }

        /// <summary>
        /// Applies discount code to cart
        /// User must have an active cart. Request must contain cartCookie header if user is not logged in.
        /// </summary>
        /// <param name="discountCode">Discount code</param>
        /// <returns></returns>
        [HttpPost("Discount")]
        public async Task<IActionResult> AddDiscountCodeAsync([FromBody] string discountCode)
        {
            try
            {
                var cartId = Request.Headers.TryGetValue(CookieConstants.CartCookie, out StringValues headerValues)
                                 ? Guid.Parse(headerValues.FirstOrDefault())
                                 : (await _shoppingCartRepository.GetCartByUserIdAsync(User.Identity.GetUserId()))?.CartId;

                if (cartId == null)
                {
                    return BadRequest("User has no active cart");
                }

                var result = await _shoppingCartService.ApplyDiscountCodeAsync(cartId.Value, discountCode);

                return result is false ? BadRequest("Bad discount code") : Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occurred in Shopping Cart Controller, Add shopping cart item");
                return BadRequest();
            }
        }

        /// <summary>
        /// Removes item from active cart
        /// User must have an active cart. Request must contain cartCookie
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpDelete("{itemId}")]
        public async Task<IActionResult> RemoveItemFromCartAsync([FromRoute] Guid itemId)
        {
            try
            {
                var cartId =  Request.Headers.TryGetValue(CookieConstants.CartCookie, out StringValues headerValues)
                                 ? Guid.Parse(headerValues.FirstOrDefault())
                                 : (await _shoppingCartRepository.GetCartByUserIdAsync(User.Identity.GetUserId()))?.CartId;

                if (cartId == null)
                {
                    return BadRequest("User has no active cart");
                }

                var result = await _shoppingCartService.RemoveItemFromCartAsync(cartId.Value, itemId);

                return result is false ? BadRequest() : Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occurred in Shopping Cart Controller, Remove shopping cart item");
                return BadRequest();
            }
        }
    }
}