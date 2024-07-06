namespace Iwan.Shared.Options.Products
{
    public class GetProductsOptions : PagedOptions
    {
        public string Text { get; set; }
        public string ParentCategoryId { get; set; }
        public string SubCategoryId { get; set; }
    }
}
