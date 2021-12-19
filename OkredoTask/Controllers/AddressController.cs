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
    public class AddressController : BaseApiController
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IAddressService _addressService;
        private readonly ILogger<OrdersManagement> _logger;

        public AddressController(
            ILogger<OrdersManagement> logger,
            IAddressRepository addressRepository,
            IAddressService addressService)
        {
            _logger = logger;
            _addressRepository = addressRepository;
            _addressService = addressService;
        }

        /// <summary>
        /// Gets user addresses
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserAddressesAsync()
        {
            try
            {
                var addresses = await _addressRepository.GetUserAddressesAsync(User.Identity.GetUserId());

                return Ok(AddressViewModel.ToModel(addresses));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Address Controller, get user address");
                return BadRequest();
            }
        }

        /// <summary>
        /// Add new address to user
        /// </summary>
        /// <param name="addressModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateNewAddressAsync([FromBody] CreateAddressModel addressModel)
        {
            try
            {
                await _addressService.AddNewAddressAsync(User.Identity.GetUserId(), addressModel.Street,
                    addressModel.City, addressModel.PostalCode);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Address Controller, when adding address");
                return BadRequest();
            }
        }

        /// <summary>
        /// Update address
        /// </summary>
        /// <returns></returns>
        [HttpPatch("{addressId}")]
        public async Task<IActionResult> UpdateUserAddressAsync([FromRoute] Guid addressId,
            [FromBody] CreateAddressModel addressModel)
        {
            try
            {
                var success = await _addressService.UpdateAddressAsync(User.Identity.GetUserId(), addressId,
                    addressModel.Street, addressModel.City, addressModel.PostalCode);

                return success ? Ok() : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Address Controller, when updating address");
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete user address
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{addressId}")]
        public async Task<IActionResult> DeleteUserAddressAsync([FromRoute] Guid addressId)
        {
            try
            {
                var success = await _addressService.RemoveAddressAsync(User.Identity.GetUserId(), addressId);

                return success ? Ok() : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Address Controller, when deleting address");
                return BadRequest();
            }
        }
    }
}