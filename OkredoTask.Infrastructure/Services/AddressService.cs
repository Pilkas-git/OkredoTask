using OkredoTask.Core.Entities;
using OkredoTask.Infrastructure.Data;
using OkredoTask.Infrastructure.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Services
{
    public class AddressService : IAddressService
    {
        private readonly AppDbContext _dbContext;
        private readonly IAddressRepository _addressRepository;

        public AddressService(AppDbContext context,
            IAddressRepository addressRepository)
        {
            _dbContext = context;
            _addressRepository = addressRepository;
        }

        public async Task<Guid> AddNewAddressAsync(Guid userId, string street, string city, string postalCode)
        {
            var newAddress = new Address(userId, street, city, postalCode);
            await _dbContext.AddAsync(newAddress);

            await _dbContext.SaveChangesAsync();
            return newAddress.AddressId;
        }

        public async Task<bool> RemoveAddressAsync(Guid userId, Guid addressId)
        {
            var addressToRemove = await _addressRepository.GetUserAddressByIdAsync(userId, addressId);

            if (addressToRemove == null)
            {
                return false;
            }

            _dbContext.Addresses.Remove(addressToRemove);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAddressAsync(Guid userId, Guid addressId, string street, string city, string postalCode)
        {
            var addressToUpdate = await _addressRepository.GetUserAddressByIdAsync(userId, addressId);

            if (addressToUpdate == null)
            {
                return false;
            }

            addressToUpdate.ChangeAddress(street, city, postalCode);
            _dbContext.Entry(addressToUpdate).CurrentValues.SetValues(addressToUpdate);

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}