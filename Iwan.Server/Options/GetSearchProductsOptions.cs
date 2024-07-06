using Iwan.Shared.Options;

namespace Iwan.Server.Options
{
    public class GetSearchProductsOptions : PagedOptions
    {
        public string Text { get; set; } = "";
    }
}
