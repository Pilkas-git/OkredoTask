using OkredoTask.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure
{
    public interface IAddressRepository
    {
        public Task<List<Address>> GetUserAddressesAsync(Guid userId);

        public Task<Address> GetAddressByIdAsync(Guid addressId);

        public Task<Address> GetUserAddressByIdAsync(Guid userId, Guid addressId);
    }
}