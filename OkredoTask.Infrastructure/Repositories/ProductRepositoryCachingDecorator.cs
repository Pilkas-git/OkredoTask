using Microsoft.Extensions.Caching.Memory;
using OkredoTask.Core.Constants;
using OkredoTask.Core.Entities;
using OkredoTask.Core.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Repositories
{
    public class ProductRepositoryCachingDecorator : IProductRepository
    {
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _memoryCache;

        public ProductRepositoryCachingDecorator(IProductRepository productRepository,
            IMemoryCache memoryCache)
        {
            _productRepository = productRepository;
            _memoryCache = memoryCache;
        }

        public async Task<List<ProductType>> GetAvailableProductTypesAsync()
        {
            if (_memoryCache.TryGetValue<List<ProductType>>(CacheConstants.CategoryCacheKey, out var data))
            {
                return data;
            }

            var productTypes = await _productRepository.GetAvailableProductTypesAsync();

            _memoryCache.Set(CacheConstants.CategoryCacheKey, productTypes, CacheConstants.CachedTime);

            return productTypes;
        }

        public async Task<Product> GetProductByIdAsync(Guid productId)
        {
            return await _productRepository.GetProductByIdAsync(productId);
        }

        public async Task<List<Product>> GetProductsByFiltersAsync(string searchText, ProductType? productType)
        {
            return await _productRepository.GetProductsByFiltersAsync(searchText, productType);
        }
    }
}