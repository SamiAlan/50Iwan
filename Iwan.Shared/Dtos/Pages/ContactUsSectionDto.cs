using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Pages
{
    public class ContactUsSectionDto
    {
        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("facebookUrl")]
        public string FacebookUrl { get; set; }

        [JsonPropertyName("instagramUrl")]
        public string InstagramUrl { get; set; }
    }
}
