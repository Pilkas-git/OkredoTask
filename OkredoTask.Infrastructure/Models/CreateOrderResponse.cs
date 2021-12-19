using System;

namespace OkredoTask.Infrastructure.Models
{
    public class CreateOrderResponse : BaseResponse
    {
        public Guid? OrderId { get; set; }

        public CreateOrderResponse(Guid orderId)
        {
            Success = true;
            OrderId = orderId;
        }

        public CreateOrderResponse(string error) : base(error)
        {
        }
    }
}