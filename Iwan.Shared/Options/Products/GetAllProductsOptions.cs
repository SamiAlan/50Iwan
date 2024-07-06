namespace Iwan.Shared.Options.Products
{
    public class GetAllProductsOptions
    {
        public string Text { get; set; }
        public bool? OnlyVisible { get; set; }
        public bool? OnlyUnattached { get; set; }
        public string UnderParentCategoryId { get; set; }
        public string UnderSubCategoryId { get; set; }
    }
}
