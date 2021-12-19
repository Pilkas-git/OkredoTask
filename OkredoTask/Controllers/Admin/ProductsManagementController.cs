using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OkredoTask.Core.Constants;
using OkredoTask.Infrastructure;
using OkredoTask.Infrastructure.Services.Interfaces;
using OkredoTask.Web.Api;
using OkredoTask.Web.Models;
using OkredoTask.Web.Patch;
using System;
using System.Threading.Tasks;

namespace OkredoTask.Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = RoleConstants.Admin)]
    [Route("Admin/Products")]
    public class ProductsManagementController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsManagementController> _logger;

        public ProductsManagementController(
            ILogger<ProductsManagementController> logger,
            IProductService productService)
        {
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Creates product
        /// </summary>
        /// <param name="createProductModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductModel createProductModel)
        {
            try
            {
                await _productService.CreateProductAsync(createProductModel.SupplierId, createProductModel.Name, createProductModel.Price,
                    createProductModel.Description, createProductModel.ProductType);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occured in Product Controller, CreateProductAsync");
                return BadRequest();
            }
        }

        /// <summary>
        /// Update product details
        /// </summary>
        /// <returns></returns>
        [HttpPatch("{productId}")]
        public async Task<IActionResult> PatchProductDetailsAsync([FromRoute] Guid productId, [FromBody] ProductPatch productPatch)
        {
            try
            {
                var response = await _productService.UpdateProductAsync(productId, productPatch.Name, productPatch.Price,
                    productPatch.Description, productPatch.Quantity);

                return response.Success ? Ok() : BadRequest(response.Error);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Product Controller, when updating product details");
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete product
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] Guid productId)
        {
            try
            {
                var success = await _productService.RemoveProductAsync(productId);

                return success ? Ok() : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Product Controller, when deleting product");
                return BadRequest();
            }
        }
    }
}