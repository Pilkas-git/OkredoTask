using Microsoft.EntityFrameworkCore;
using OkredoTask.Core.Entities;
using OkredoTask.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _dbContext;

        public AddressRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<Address>> GetUserAddressesAsync(Guid userId)
        {
            return await _dbContext.Addresses.Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<Address> GetAddressByIdAsync(Guid addressId)
        {
            return await _dbContext.Addresses.FirstOrDefaultAsync(a => a.AddressId == addressId);
        }

        public async Task<Address> GetUserAddressByIdAsync(Guid userId, Guid addressId)
        {
            return await _dbContext.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == addressId && a.UserId == userId);
        }
    }
}