using Microsoft.Extensions.Logging;
using OkredoTask.Core.Entities;
using OkredoTask.Core.Enums;
using OkredoTask.Infrastructure.Data;
using OkredoTask.Infrastructure.Models;
using OkredoTask.Infrastructure.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(AppDbContext context,
            IProductRepository productRepository,
            ILogger<ProductService> logger)
        {
            _dbContext = context;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<int> CreateProductAsync(Guid supplierId, string name, decimal price, string description,
            ProductType productType)
        {
            var product = new Product(supplierId, name, price, description, productType);
            await _dbContext.AddAsync(product);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<BaseResponse> UpdateProductAsync(Guid id, string name, decimal? price, string description,
            int? quantity)
        {
            var productToUpdate = await _productRepository.GetProductByIdAsync(id);
            if (productToUpdate == null)
            {
                return new BaseResponse("Product not found");
            }
            if (quantity < 0)
            {
                return new BaseResponse("Invalid product quantity");
            }
            if (price < 0)
            {
                return new BaseResponse("Invalid product price");
            }

            try
            {
                productToUpdate.UpdateProductDetails(name, price, description, quantity);
                _dbContext.Entry(productToUpdate).CurrentValues.SetValues(productToUpdate);

                await _dbContext.SaveChangesAsync();
                return new BaseResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occurred updating product");
                return new BaseResponse("Unable to save changes. Product was deleted by another user.");
            }
        }

        public async Task<bool> RemoveProductAsync(Guid productId)
        {
            var productToRemove = await _productRepository.GetProductByIdAsync(productId);

            if (productToRemove == null)
            {
                return false;
            }

            _dbContext.Products.Remove(productToRemove);

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}