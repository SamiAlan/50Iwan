using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Products
{
    public class ProductCategoryDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("englishName")]
        public string EnglishName { get; set; }

        [JsonPropertyName("arabicName")]
        public string ArabicName { get; set; }

        [JsonPropertyName("categoryId")]
        public string CategoryId { get; set; }
    }
}
