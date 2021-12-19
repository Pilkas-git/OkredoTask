using System;
using System.Collections.Generic;

namespace OkredoTask.Core.Entities
{
    public class Supplier : PersistentEntityBase
    {
        public Guid SupplierId { get; private set; }
        public string SupplierName { get; private set; }

        private readonly List<Product> _products = new();
        public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

        private Supplier()
        {
        }

        public Supplier(string supplierName)
        {
            SupplierName = supplierName;
        }
    }
}