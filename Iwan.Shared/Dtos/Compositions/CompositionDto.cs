using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Compositions
{
    public class CompositionDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("arabicName")]
        public string ArabicName { get; set; }

        [JsonPropertyName("englishName")]
        public string EnglishName { get; set; }

        [JsonPropertyName("arabicDescription")]
        public string ArabicDescription { get; set; }

        [JsonPropertyName("englishDescription")]
        public string EnglishDescription { get; set; }

        [JsonPropertyName("useBackgroundlessImage")]
        public bool UseBackgroundlessImage { get; set; }

        [JsonPropertyName("isVisible")]
        public bool IsVisible { get; set; }

        [JsonPropertyName("colorCode")]
        public string ColorCode { get; set; }

        [JsonPropertyName("colorTypeId")]
        public int ColorTypeId { get; set; }

        [JsonPropertyName("image")]
        public CompositionImageDto Image { get; set; }
    }
}
