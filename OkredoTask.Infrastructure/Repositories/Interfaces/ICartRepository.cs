using OkredoTask.Core.Entities;
using System;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Repositories.Interfaces
{
    public interface ICartRepository
    {
        public Task<Cart> GetCartByIdAsync(Guid id);

        public Task<Cart> GetCartWithItemsByIdAsync(Guid id);

        public Task<Cart> GetCartByUserIdAsync(Guid userId);

        public Task<Cart> GetCartWithItemsByUserIdAsync(Guid userId);
    }
}