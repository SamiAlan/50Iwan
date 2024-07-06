namespace Iwan.Shared.Options.Products
{
    public class AdminGetProductsOptions : PagedOptions
    {
        public int? Number { get; set; }
        public string Text { get; set; }
        public bool IncludeMainImage { get; set; }
        public bool? OnlyVisible { get; set; }
        public bool? OnlyUnattached { get; set; }
        public bool? HavingNoMainImage { get; set; }
        public bool? OnlyOwnedProducts { get; set; }
        public string UnderCategoryId { get; set; }
        public string UnderSubCategoryId { get; set; }
        public string UnderVendorId { get; set; }
        public bool? OnlyNeedingResize { get; set; }
    }
}
