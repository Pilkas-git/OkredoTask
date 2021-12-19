using OkredoTask.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OkredoTask.Web.Models
{
    public class UserViewModel
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserRole { get; set; }

        public static UserViewModel ToModel(User user)
        {
            return new UserViewModel
            {
                UserId = user.UserId,
                Name = user.FirstName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserRole = user.UserRole.ToString()
            };
        }

        public static List<UserViewModel> ToModel(List<User> users)
        {
            return users.Select(user => ToModel(user)).ToList();
        }
    }
}