namespace Iwan.Shared.Options.Catalog
{
    public class GetAllCategoriesOptions
    {
        public CategoryType? Type { get; set; }
        public bool? OnlyVisible { get; set; }
        public bool? OnlyHasRelatedProducts { get; set; }
        public string UnderParentCategoryId { get; set; }
        public bool IncludeImages { get; set; }
        public string Text { get; set; }
    }
}
