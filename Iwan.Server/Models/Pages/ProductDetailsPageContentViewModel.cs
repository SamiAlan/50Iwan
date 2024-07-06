using Iwan.Server.Models.Common;
using Iwan.Server.Models.Products;
using System.Collections.Generic;
using System.Linq;

namespace Iwan.Server.Models.Pages
{
    public class ProductDetailsPageContentViewModel : PageViewModel
    {
        public string SelectedCategoryName { get; set; }
        public string SelectedSubCategoryName { get; set; }
        public string SelectedCategoryId { get; set; }
        public string SelectedSubCategoryId { get; set; }
        public string RecepientEmail { get; set; }
        public string PhoneNumber { get; set; }
        public ProductViewModel Product { get; set; }
        public List<ProductStateViewModel> States { get; set; }
        public List<ProductImageViewModel> Images { get; set; }
        public List<SimilarProductViewModel> SimilarProducts { get; set; }
        public List<ColorViewModel> Colors { get; set; }

        public IEnumerable<string> StatesAsString => States.Select(s => s.Text);
    }
}
