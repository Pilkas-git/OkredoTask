using OkredoTask.Infrastructure.Models;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Services.Interfaces
{
    public interface IIdentityService
    {
        public Task<RegistrationResponse> RegisterAsync(string email, string firstName, string lastName, string password);

        public Task<(string token, string error)> LoginAsync(string email, string password);
    }
}