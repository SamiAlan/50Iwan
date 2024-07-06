using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Products
{
    public class ProductStateDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("arabicName")]
        public string ArabicName { get; set; }

        [JsonPropertyName("englishName")]
        public string EnglishName { get; set; }

        [JsonPropertyName("productId")]
        public string ProductId { get; set; }
    }
}
