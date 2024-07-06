namespace Iwan.Shared.Dtos.Catalog
{
    public class EditCategoryDto
    {
        public string Id { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public bool IsVisible { get; set; }
        public string ParentCategoryId { get; set; }
        public string ColorCode { get; set; }
        public int ColorTypeId { get; set; }
    }
}
