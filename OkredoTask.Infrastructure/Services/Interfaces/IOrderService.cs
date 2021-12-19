using OkredoTask.Infrastructure.CustomModels;
using OkredoTask.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<CreateOrderResponse> CreateOrderAsync(CreateOrderModel orderModel);

        public Task<bool> ChangeOrderStatusAsync(Guid orderId, string orderStatus);
    }
}