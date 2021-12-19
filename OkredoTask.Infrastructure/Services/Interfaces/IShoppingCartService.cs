using OkredoTask.Core.Entities;
using System;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Services.Interfaces
{
    public interface IShoppingCartService
    {
        public Task<Cart> CreateCartAsync();

        public Task<Cart> GetUserCartAsync(Guid userId);

        public Task<bool> AddItemToCartAsync(Guid cartId, Guid itemId, int quantity);

        public Task<bool> ApplyDiscountCodeAsync(Guid cartId, string discountCode);

        public Task<bool> RemoveItemFromCartAsync(Guid cartId, Guid itemId);
    }
}