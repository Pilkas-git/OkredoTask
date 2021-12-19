using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OkredoTask.Core.Entities;
using OkredoTask.Infrastructure.CustomModels;
using OkredoTask.Infrastructure.Data;
using OkredoTask.Infrastructure.Models;
using OkredoTask.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Services.Interfaces
{
    public abstract class OrderCreatorStrategy
    {
        private readonly AppDbContext _dbContext;
        private readonly ICartRepository _shoppingCartRepository;
        private readonly ILogger<OrderCreatorStrategy> _logger;
        private readonly ICartItemService _cartItemService;

        protected OrderCreatorStrategy(
            AppDbContext dbContext,
            ICartRepository shoppingCartRepository,
            ILogger<OrderCreatorStrategy> logger,
            ICartItemService cartItemService)
        {
            _dbContext = dbContext;
            _shoppingCartRepository = shoppingCartRepository;
            _logger = logger;
            _cartItemService = cartItemService;
        }

        public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderModel orderModel)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            Guid orderId = Guid.Empty;
            try
            {
                var cart = await GetCartAsync(orderModel.UserId, orderModel.CartId);

                if (cart is null)
                {
                    return new CreateOrderResponse("User has no cart.");
                }
                if (cart?.CartItems == null || !cart.CartItems.Any())
                {
                    return new CreateOrderResponse("Cart cannot be empty.");
                }

                (var userId, var addressId) = await SetupDeliveryAsync(orderModel);

                cart.SetUser(userId);
                cart.UpdateCartPrice();

                var order = new Order(userId, addressId, orderModel.Comment, cart.Price);
                orderId = order.OrderId;
                if (cart.Discount != null)
                {
                    order.AddUsedDiscount(cart.Discount);
                    if (cart.Discount.IsSingleUse)
                    {
                        cart.Discount.Deactivate();
                    }
                }

                _cartItemService.ReduceProductAvailability(cart);

                UpdateOrderItemsFromCart(cart.CartItems.ToList(), order, cart);
                await _dbContext.Orders.AddAsync(order);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new CreateOrderResponse(orderId); //TODO Return user redirect to payment handler, handle payment callback
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is Product proposedValues)
                    {
                        var original = (Product)entry.OriginalValues.ToObject();
                        var databaseValues = (Product)entry.GetDatabaseValues().ToObject();

                        //Handle calculating new availabilityCount
                        var quantityReducedBy = original.AvailabilityCount - proposedValues.AvailabilityCount;
                        var correctQuantity = databaseValues.AvailabilityCount - quantityReducedBy;
                        if (correctQuantity < 0)
                        {
                            _logger.LogError("Product in cart is not in stock product name:{name}", proposedValues.Name);

                            await transaction.RollbackAsync();
                            return new CreateOrderResponse($"Failed to create order, Product {proposedValues.Name}" +
                                $" in cart is not in stock");
                        }
                        proposedValues.SetAvailabilityCount(correctQuantity);

                        // Refresh original values to bypass next concurrency check
                        entry.OriginalValues.SetValues(databaseValues);
                    }
                    else
                    {
                        throw new NotSupportedException("Don't know how to handle concurrency conflicts for "
                            + entry.Metadata.Name);
                    }
                }
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new CreateOrderResponse(orderId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in OrderService: CreateOrder");

                await transaction.RollbackAsync();
                return new CreateOrderResponse("Exception occurred in OrderService: CreateOrder");
            }
        }

        public abstract Task<(Guid userId, Guid addressId)> SetupDeliveryAsync(CreateOrderModel orderModel);

        private async Task<Cart> GetCartAsync(Guid? userId, Guid? cartId)
        {
            Cart cart = null;
            if (userId.HasValue)
            {
                cart = await _shoppingCartRepository.GetCartWithItemsByUserIdAsync(userId.Value);
            }
            else if (cartId.HasValue)
            {
                cart = await _shoppingCartRepository.GetCartWithItemsByIdAsync(cartId.Value);
            }

            return cart;
        }

        private void UpdateOrderItemsFromCart(List<CartItem> cartItems, Order order, Cart cart)
        {
            var orderItems = CartItem.ToOrderItems(cartItems, order);
            order.AddOrderItems(orderItems);

            _dbContext.RemoveRange(cart.CartItems);
            _dbContext.Carts.Remove(cart);
            _dbContext.Orders.Update(order);
        }
    }
}