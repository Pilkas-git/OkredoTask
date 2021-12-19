using OkredoTask.Core.Enums;
using System;

namespace OkredoTask.Core.Entities
{
    public class Product : PersistentEntityBase
    {
        public Guid ProductId { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public int AvailabilityCount { get; private set; }
        public ProductType ProductType { get; private set; }
        public Guid SupplierId { get; private set; }
        public Supplier Supplier { get; private set; }

        private Product()
        {
        }

        public Product(Guid supplierId, string name, decimal price, string description, ProductType productType)
        {
            SupplierId = supplierId;
            Name = name;
            Price = price;
            Description = description;
            ProductType = productType;
        }

        public void SetAvailabilityCount(int newCount)
        {
            AvailabilityCount = newCount;
        }

        public void UpdateProductDetails(string name, decimal? price, string description, int? quantity)
        {
            Name = name;
            Description = description;

            if (price.HasValue)
            {
                Price = (decimal)price;
            }

            if (quantity.HasValue)
            {
                AvailabilityCount = (int)quantity;
            }
        }
    }
}