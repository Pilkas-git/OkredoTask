using Microsoft.Extensions.Logging;
using OkredoTask.Core.Entities;
using OkredoTask.Infrastructure.Data;
using OkredoTask.Infrastructure.Repositories.Interfaces;
using OkredoTask.Infrastructure.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly AppDbContext _dbContext;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _shoppingCartRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<ShoppingCartService> _logger;

        public ShoppingCartService(AppDbContext dbContext,
            IProductRepository productRepository,
            ILogger<ShoppingCartService> logger,
            ICartRepository shoppingCartRepository,
            IDiscountRepository discountRepository)
        {
            _dbContext = dbContext;
            _productRepository = productRepository;
            _logger = logger;
            _shoppingCartRepository = shoppingCartRepository;
            _discountRepository = discountRepository;
        }

        public async Task<Cart> CreateCartAsync()
        {
            var cart = new Cart();

            await _dbContext.AddAsync(cart);
            await _dbContext.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart> GetUserCartAsync(Guid userId)
        {
            try
            {
                var existingCart = await _shoppingCartRepository.GetCartWithItemsByUserIdAsync(userId);
                if (existingCart != null)
                {
                    return existingCart;
                }

                var cart = new Cart();
                cart.SetUser(userId);

                await _dbContext.AddAsync(cart);
                await _dbContext.SaveChangesAsync();
                return cart;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in ShoppingCartService: GetUserCart");
                throw;
            }
        }

        public async Task<bool> AddItemToCartAsync(Guid cartId, Guid itemId, int quantity)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(itemId);
                if (product == null || product.AvailabilityCount <= 0 || quantity <= 0)
                {
                    return false;
                }

                var cart = await _shoppingCartRepository.GetCartWithItemsByIdAsync(cartId);
                if (cart.CartItems.Any(x => x.ProductId == itemId))
                {
                    var cartItem = cart.CartItems.First(x => x.ProductId == itemId);
                    cartItem.SetQuantity(quantity);
                    cart.UpdateCartPrice();
                }
                else
                {
                    cart.AddCartItem(product, quantity);
                }

                _dbContext.Carts.Update(cart);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in ShoppingCartService: AddItemToCart");
                return false;
            }
        }

        public async Task<bool> RemoveItemFromCartAsync(Guid cartId, Guid itemId)
        {
            try
            {
                var cart = await _shoppingCartRepository.GetCartWithItemsByIdAsync(cartId);
                cart.RemoveCartItem(itemId);

                _dbContext.Carts.Update(cart);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in ShoppingCartService: RemoveItemFromCart");
                return false;
            }
        }

        public async Task<bool> ApplyDiscountCodeAsync(Guid cartId, string discountCode)
        {
            try
            {
                var cart = await _shoppingCartRepository.GetCartWithItemsByIdAsync(cartId);
                var discount = await _discountRepository.GetDiscountByCodeAsync(discountCode);

                if (discount == null || cart == null)
                {
                    _logger.LogError("Failed to apply discount code cart or discount code is null");
                    return false;
                }

                if (!discount.IsValid || discount.Expires < DateTime.UtcNow)
                {
                    return false;
                }

                cart.ApplyDiscountCode(discount);

                _dbContext.Discounts.Update(discount);
                _dbContext.Carts.Update(cart);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in ShoppingCartService: RemoveItemFromCart");
                return false;
            }
        }
    }
}