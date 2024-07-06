using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Pages
{
    public class HomePageContentDto
    {
        [JsonPropertyName("headerSection")]
        public HeaderSectionDto HeaderSection { get; set; } = new();

        [JsonPropertyName("servicesSection")]
        public ServicesSectionDto ServicesSection { get; set; } = new();

        [JsonPropertyName("contactUsSection")]
        public ContactUsSectionDto ContactUsSection { get; set; } = new();

        [JsonPropertyName("aboutUsSection")]
        public AboutUsSectionDto AboutUsSection { get; set; } = new();

        [JsonPropertyName("interiorDesignSection")]
        public InteriorDesignSectionDto InteriorDesignSection { get; set; } = new();
    }
}
