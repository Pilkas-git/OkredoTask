using System;

namespace OkredoTask.Core.Entities
{
    public class Address : PersistentEntityBase
    {
        public Guid AddressId { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string PostalCode { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        private Address()
        {
        }

        public Address(Guid userId, string street, string city, string postalCode)
        {
            UserId = userId;
            Street = street;
            City = city;
            PostalCode = postalCode;
        }

        public void ChangeAddress(string street, string city, string postalCode)
        {
            Street = street;
            City = city;
            PostalCode = postalCode;
        }
    }
}