using System;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        public Task<Guid> AddUserAsync(string firstName, string lastName, string email);
    }
}