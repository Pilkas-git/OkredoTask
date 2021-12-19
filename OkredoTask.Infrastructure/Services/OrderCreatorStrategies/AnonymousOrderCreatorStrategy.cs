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
    public class AnonymousOrderCreatorStrategy : OrderCreatorStrategy
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<AnonymousOrderCreatorStrategy> _logger;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public AnonymousOrderCreatorStrategy(
            AppDbContext dbContext,
            ILogger<AnonymousOrderCreatorStrategy> logger,
            IUserService userService,
            IUserRepository userRepository,
            ICartItemService cartItemService,
            ICartRepository shoppingCartRepository) : base(dbContext, shoppingCartRepository, logger, cartItemService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _userService = userService;
            _userRepository = userRepository;
        }

        public async override Task<(Guid userId, Guid addressId)> SetupDeliveryAsync(CreateOrderModel orderModel)
        {
            _logger.LogDebug("Starting to handle SetupUser for anonymous order");
            var user = await _userRepository.GetUserByEmailAsync(orderModel.Email);
            if (user is null)
            {
                var userId = await _userService.AddUserAsync(orderModel.FirstName, orderModel.LastName, orderModel.Email);
                user = await _userRepository.GetUserByIdAsync(userId);
            }

            user.SetPhoneNumber(orderModel.PhoneNumber);
            var address = new Address(user.UserId, orderModel.Address, orderModel.City, orderModel.PostCode);
            user.AddAddress(address);
            await _dbContext.SaveChangesAsync();

            _logger.LogDebug("Finished handling SetupUser for anonymous order");
            return (user.UserId, address.AddressId);
        }
    }
}