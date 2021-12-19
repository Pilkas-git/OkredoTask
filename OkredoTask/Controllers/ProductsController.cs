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
using System.Linq;
using System.Threading.Tasks;

namespace OkredoTask.Web.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository productRepository,
            ILogger<ProductsController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        /// <summary>
        /// Gets product by provided product Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductAsync([FromRoute] Guid productId)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(productId);

                return product is null ? BadRequest() : Ok(ProductViewModel.ToModel(product));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occured in Product Controller, get product");
                return BadRequest();
            }
        }

        /// <summary>
        /// Gets category types which have a product
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/Categories")]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            try
            {
                var categories = await _productRepository.GetAvailableProductTypesAsync(); 

                return categories.Any() ? Ok(CategoryViewModel.ToModel(categories)) : BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occured in Product Controller, get product");
                return BadRequest();
            }
        }

        /// <summary>
        /// Gets all filtered products if no filtering parameters are given returns all products
        /// </summary>
        /// <param name="productFiltersModel">Query filters</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetProductsAsync([FromQuery] ProductFiltersModel productFiltersModel)
        {
            try
            {
                var product = await _productRepository.GetProductsByFiltersAsync(productFiltersModel.SearchText,
                    productFiltersModel.ProductType);

                return product is null ? BadRequest() : Ok(ProductViewModel.ToModel(product));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occured in Product Controller, GetProductsAsync");
                return BadRequest();
            }
        }
    }
}