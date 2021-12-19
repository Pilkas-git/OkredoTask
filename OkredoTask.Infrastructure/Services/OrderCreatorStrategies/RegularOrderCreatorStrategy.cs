using Microsoft.Extensions.Logging;
using OkredoTask.Core.Entities;
using OkredoTask.Infrastructure.CustomModels;
using OkredoTask.Infrastructure.Data;
using OkredoTask.Infrastructure.Repositories.Interfaces;
using OkredoTask.Infrastructure.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Services
{
    public class RegularOrderCreatorStrategy : OrderCreatorStrategy
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<RegularOrderCreatorStrategy> _logger;
        private readonly IUserRepository _userRepository;

        public RegularOrderCreatorStrategy(
            AppDbContext dbContext,
            ILogger<RegularOrderCreatorStrategy> logger,
            IUserRepository userRepository,
            ICartItemService cartItemService,
            ICartRepository shoppingCartRepository) : base(dbContext, shoppingCartRepository, logger, cartItemService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async override Task<(Guid userId, Guid addressId)> SetupDeliveryAsync(CreateOrderModel orderModel)
        {
            _logger.LogDebug("Starting to handle userSetup for anonymous order");
            var user = await _userRepository.GetUserByIdWithAddressAsync(orderModel.UserId.Value);
            user.SetPhoneNumber(orderModel.PhoneNumber);

            var addressId = orderModel.AddressId;
            if (addressId == null)
            {
                var address = new Address(user.UserId, orderModel.Address, orderModel.City, orderModel.PostCode);
                user.AddAddress(address);

                await _dbContext.SaveChangesAsync();
                addressId = address.AddressId;
            }

            _logger.LogDebug("Finished handling userSetup for anonymous order");
            return (user.UserId, addressId.Value);
        }
    }
}