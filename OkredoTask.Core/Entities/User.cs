using OkredoTask.Core.Enums;
using System;
using System.Collections.Generic;

namespace OkredoTask.Core.Entities
{
    public class User : PersistentEntityBase
    {
        public Guid UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public UserRole UserRole { get; private set; }

        private readonly List<Address> _addresses = new();
        public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();

        private readonly List<Order> _orders = new();
        public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

        private User()
        {
        }

        public User(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserRole = UserRole.Client;
        }

        public void SetPhoneNumber(string phoneNumber) => PhoneNumber = phoneNumber;

        public void SetUserRole(UserRole userRole) => UserRole = userRole;

        public void AddAddress(Address address) => _addresses.Add(address);
    }
}