using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OkredoTask.Core.Constants;
using OkredoTask.Infrastructure.Repositories.Interfaces;
using OkredoTask.Infrastructure.Services.Interfaces;
using OkredoTask.Web.Api;
using OkredoTask.Web.Models;
using System;
using System.Threading.Tasks;

namespace OkredoTask.Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = RoleConstants.Admin)]
    [Route("Admin/Discounts")]
    public class DiscountsController : BaseApiController
    {
        private readonly ILogger<DiscountsController> _logger;
        private readonly IDiscountRepository _discountRepository;
        private readonly IDiscountService _discountService;

        public DiscountsController(
            ILogger<DiscountsController> logger,
            IDiscountRepository discountRepository,
            IDiscountService discountService)
        {
            _logger = logger;
            _discountRepository = discountRepository;
            _discountService = discountService;
        }

        /// <summary>
        /// Gets discount codes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDiscountCodesAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                var discounts = await _discountRepository.GetDiscountListAsync(pageNumber, pageSize);

                return Ok(DiscountViewModel.ToModel(discounts));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Discount Management Controller, get discount codes");
                return BadRequest();
            }
        }

        /// <summary>
        /// Add new discount code
        /// </summary>
        /// <param name="discountModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateNewDiscountAsync([FromBody] CreateDiscountModel discountModel)
        {
            try
            {
                await _discountService.AddNewDiscountCodeAsync(discountModel.DiscountCode, discountModel.DiscountValue, discountModel.IsSingleUse,
                    discountModel.IsFixedValue, discountModel.ExpiresAt);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Discount Management Controller, when creating discount");
                return BadRequest();
            }
        }

        /// <summary>
        /// Deactivate discount code
        /// </summary>
        /// <returns></returns>
        [HttpPatch("{discountId}")]
        public async Task<IActionResult> DeactivateDiscountAsync([FromRoute] Guid discountId)
        {
            try
            {
                var success = await _discountService.DeactivateDiscountAsync(discountId);

                return success ? Ok() : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Discount Management Controller, when deactivating discount");
                return BadRequest();
            }
        }

        /// <summary>
        /// Update discount code
        /// </summary>
        /// <returns></returns>
        [HttpPut("{discountId}")]
        public async Task<IActionResult> UpdateDiscountAsync([FromRoute] Guid discountId, [FromBody] CreateDiscountModel discountModel)
        {
            try
            {
                var success = await _discountService.UpdateDiscountAsync(discountId, discountModel.DiscountCode, discountModel.DiscountValue,
                    discountModel.IsSingleUse, discountModel.IsFixedValue, discountModel.ExpiresAt);

                return success ? Ok() : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Discount Management Controller, when updating discount");
                return BadRequest();
            }
        }
    }
}