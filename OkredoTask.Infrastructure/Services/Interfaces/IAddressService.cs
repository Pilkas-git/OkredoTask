using System;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Services.Interfaces
{
    public interface IAddressService
    {
        public Task<Guid> AddNewAddressAsync(Guid userId, string street, string city, string postalCode);

        public Task<bool> RemoveAddressAsync(Guid userId, Guid addressId);

        public Task<bool> UpdateAddressAsync(Guid userId, Guid addressId, string street, string city, string postalCode);
    }
}