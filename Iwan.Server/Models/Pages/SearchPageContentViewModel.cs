using Iwan.Server.Models.Products;

namespace Iwan.Server.Models.Pages
{
    public class SearchPageContentViewModel : PageViewModel
    {
        public string Text { get; set; }
        public PagedViewModel<ProductViewModel> Products { get; set; }
    }
}
