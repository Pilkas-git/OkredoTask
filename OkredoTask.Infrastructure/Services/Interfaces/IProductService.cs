using OkredoTask.Core.Enums;
using OkredoTask.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Services.Interfaces
{
    public interface IProductService
    {
        public Task<int> CreateProductAsync(Guid supplierId, string name, decimal price, string description, ProductType productType);

        public Task<BaseResponse> UpdateProductAsync(Guid id, string name, decimal? price, string description, int? quantity);

        public Task<bool> RemoveProductAsync(Guid productId);
    }
}