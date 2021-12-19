using OkredoTask.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OkredoTask.Web.Models
{
    public class AddressViewModel
    {
        public Guid AddressId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        public static AddressViewModel ToModel(Address address)
        {
            return new AddressViewModel
            {
                AddressId = address.AddressId,
                Street = address.Street,
                City = address.City,
                PostalCode = address.PostalCode
            };
        }

        public static List<AddressViewModel> ToModel(List<Address> addresses)
        {
            return addresses.Select(address => ToModel(address)).ToList();
        }
    }
}