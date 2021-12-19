using System;

namespace OkredoTask.Web.Post
{
    public class CreateOrderModel
    {
        /// <summary>
        /// Only required If order is created by registered user which wants to use existing address
        /// </summary>
        public Guid? AddressId { get; set; } 
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Comment { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }


        public Infrastructure.CustomModels.CreateOrderModel ToCreateOrderModelWithUserId(Guid userId)
        {
            return ToCreateOrderModel(userId, null);
        }

        public Infrastructure.CustomModels.CreateOrderModel ToCreateOrderModelWithCartId(Guid cartId)
        {
            return ToCreateOrderModel(null, cartId);
        }

        private Infrastructure.CustomModels.CreateOrderModel ToCreateOrderModel(Guid? userId, Guid? cartId)
        {
            return new Infrastructure.CustomModels.CreateOrderModel()
            {
                UserId = userId,
                AddressId = AddressId,
                Email = Email,
                Comment = Comment,
                CartId = cartId,
                FirstName = FirstName,
                LastName = LastName,
                Address = Address,
                City = City,
                PostCode = PostCode,
                PhoneNumber = PhoneNumber
            };
        }
    }
}