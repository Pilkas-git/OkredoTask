using OkredoTask.Core.Enums;

namespace OkredoTask.Web.Models
{
    public class ProductFiltersModel
    {
        public string SearchText { get; set; }
        public ProductType? ProductType { get; set; }
    }
}