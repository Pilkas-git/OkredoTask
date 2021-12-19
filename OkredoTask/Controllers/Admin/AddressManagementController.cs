using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OkredoTask.Core.Constants;
using OkredoTask.Infrastructure;
using OkredoTask.Infrastructure.Services.Interfaces;
using OkredoTask.Web.Api;
using OkredoTask.Web.Extensions;
using OkredoTask.Web.Models;
using System;
using System.Threading.Tasks;

namespace OkredoTask.Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = RoleConstants.Admin)]
    [Route("Admin/Address")]
    public class AddressManagementController : BaseApiController
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ILogger<AddressManagementController> _logger;

        public AddressManagementController(
            ILogger<AddressManagementController> logger,
            IAddressRepository addressRepository)
        {
            _logger = logger;
            _addressRepository = addressRepository;
        }

        /// <summary>
        /// Gets specified address
        /// </summary>
        /// <returns></returns>
        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetUserAddressAsync([FromRoute] Guid addressId)
        {
            try
            {
                var address = await _addressRepository.GetAddressByIdAsync(addressId);

                return address is null ? BadRequest() : Ok(AddressViewModel.ToModel(address));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Address Management Controller, get address by address id");
                return BadRequest();
            }
        }
    }
}