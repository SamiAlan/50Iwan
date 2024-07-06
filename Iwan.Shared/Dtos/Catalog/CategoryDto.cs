using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Catalog
{
    public class CategoryDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("arabicName")]
        public string ArabicName { get; set; }

        [JsonPropertyName("englishName")]
        public string EnglishName { get; set; }

        [JsonPropertyName("isVisible")]
        public bool IsVisible { get; set; }

        [JsonPropertyName("colorCode")]
        public string ColorCode { get; set; }

        [JsonPropertyName("colorTypeId")]
        public int ColorTypeId { get; set; }

        [JsonPropertyName("parentCategoryId")]
        public string ParentCategoryId { get; set; }

        [JsonPropertyName("image")]
        public CategoryImageDto Image { get; set; }
    }
}
