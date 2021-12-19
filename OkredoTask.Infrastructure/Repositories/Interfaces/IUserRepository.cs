using OkredoTask.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure
{
    public interface IUserRepository
    {
        public Task<User> GetUserByEmailAsync(string email);

        public Task<User> GetUserByIdAsync(Guid userId);

        public  Task<User> GetUserByIdWithAddressAsync(Guid userId);

        public Task<List<User>> GetUsersAsync(int pageNumber, int pageSize);
    }
}