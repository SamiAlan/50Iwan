using Iwan.Shared.Options;

namespace Iwan.Server.Options
{
    public class GetProductsPageOptions : PagedOptions
    {
        public string CategoryId { get; set; }
        public string SubCategoryId { get; set; }
    }
}
