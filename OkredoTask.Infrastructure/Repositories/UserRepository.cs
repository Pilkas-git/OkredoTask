using Microsoft.EntityFrameworkCore;
using OkredoTask.Core.Entities;
using OkredoTask.Infrastructure.Data;
using OkredoTask.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbContext.ApplicationUsers.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<User> GetUserByIdWithAddressAsync(Guid userId)
        {
            return await _dbContext.ApplicationUsers.Include(x => x.Addresses).FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<List<User>> GetUsersAsync(int pageNumber, int pageSize)
        {
            return await _dbContext.ApplicationUsers
                .AsNoTracking()
                .Page(pageNumber, pageSize)
                .ToListAsync();
        }
    }
}