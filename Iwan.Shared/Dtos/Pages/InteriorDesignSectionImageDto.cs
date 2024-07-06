using Iwan.Shared.Dtos.Media;
using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Pages
{
    public class InteriorDesignSectionImageDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("mainImage")]
        public ImageDto MainImage { get; set; }

        [JsonPropertyName("mobileImage")]
        public ImageDto MobileImage { get; set; }
    }
}
