using OkredoTask.Core.Entities;
using OkredoTask.Core.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure
{
    public interface IProductRepository
    {
        public Task<Product> GetProductByIdAsync(Guid productId);

        public Task<List<Product>> GetProductsByFiltersAsync(string searchText, ProductType? productType);

        public Task<List<ProductType>> GetAvailableProductTypesAsync();
    }
}