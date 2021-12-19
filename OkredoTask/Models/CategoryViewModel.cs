using OkredoTask.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace OkredoTask.Web.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static CategoryViewModel ToModel(ProductType productType)
        {
            return new CategoryViewModel
            {
                Id = (int)productType,
                Name = productType.ToString()
            };
        }

        public static List<CategoryViewModel> ToModel(List<ProductType> productTypes)
        {
            return productTypes.Select(productType => ToModel(productType)).ToList();
        }
    }
}