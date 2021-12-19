using Microsoft.EntityFrameworkCore;
using OkredoTask.Core.Entities;
using OkredoTask.Core.Enums;
using OkredoTask.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<ProductType>> GetAvailableProductTypesAsync()
        {
            return await _dbContext.Products
                .AsNoTracking()
                .GroupBy(x => x.ProductType)
                .Select(x => x.Key)
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(Guid productId)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task<List<Product>> GetProductsByFiltersAsync(string searchText, ProductType? productType)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .Where(x => (searchText == null || EF.Functions.Contains(x.Name, searchText)) &&
                            (productType == null || x.ProductType == productType))
                .ToListAsync();
        }
    }
}