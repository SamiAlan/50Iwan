using Iwan.Server.Models.Catalog;
using Iwan.Server.Models.Products;
using System.Collections.Generic;

namespace Iwan.Server.Models.Pages
{
    public class ProductsPageContentViewModel : PageViewModel
    {
        public string SelectedCategoryId { get; set; }
        public string SelectedCategoryName { get; set; }
        public string SelectedSubCategoryId { get; set; }
        public List<CategoryViewModel> SubCategories { get; set; }
        public PagedViewModel<ProductViewModel> Products { get; set; }
    }
}
