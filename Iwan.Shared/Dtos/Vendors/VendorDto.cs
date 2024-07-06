using Iwan.Shared.Dtos.Common;
using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Vendors
{
    public class VendorDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("showOwnProducts")]
        public bool ShowOwnProducts { get; set; }

        [JsonPropertyName("benefitPercent")]
        public decimal BenefitPercent { get; set; }

        [JsonPropertyName("address")]
        public AddressDto Address { get; set; }
    }
}
