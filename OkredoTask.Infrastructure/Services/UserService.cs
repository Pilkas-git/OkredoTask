using OkredoTask.Core.Entities;
using OkredoTask.Infrastructure.Data;
using OkredoTask.Infrastructure.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Guid> AddUserAsync(string firstName, string lastName, string email)
        {
            var newUser = new User(firstName, lastName, email);
            await _dbContext.AddAsync(newUser);

            await _dbContext.SaveChangesAsync();
            return newUser.UserId;
        }
    }
}