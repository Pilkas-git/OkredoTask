using Microsoft.Extensions.Logging;
using OkredoTask.Core.Entities;
using OkredoTask.Infrastructure.Data;
using System;
using System.Linq;

namespace OkredoTask.Infrastructure.Services
{
    public interface ICartItemService
    {
        public void ReduceProductAvailability(Cart cart);
    }

    public class CartItemService : ICartItemService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<CartItemService> _logger;

        public CartItemService(
            AppDbContext context,
            ILogger<CartItemService> logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        public async void ReduceProductAvailability(Cart cart)
        {
            _logger.LogDebug("Reducing product availability");
            var idsToReduce = cart.CartItems.Select(i => i.ProductId).ToList();
            var productsToReduce = cart.CartItems.Select(i => i.Product).Where(i => idsToReduce.Contains(i.ProductId)).ToList();

            foreach (var product in productsToReduce)
            {
                var productQuantityInCart = cart.CartItems.First(i => i.ProductId == product.ProductId).Quantity;
                var reducedAvailability = product.AvailabilityCount - productQuantityInCart;

                if (reducedAvailability < 0)
                {
                    _logger.LogError("Attempting to buy more of product than available productId:{productId}", product.ProductId);
                    throw new InvalidOperationException(
                        $"Attempting to buy more of Product with id {product.ProductId} than is currently available.\n");
                }

                product.SetAvailabilityCount(reducedAvailability);
            }

            _dbContext.Update(cart);
            await _dbContext.SaveChangesAsync();
            _logger.LogDebug("Product availability reduced");
        }
    }
}