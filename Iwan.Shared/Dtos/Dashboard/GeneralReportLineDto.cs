using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Dashboard
{
    public class GeneralReportLineDto
    {
        [JsonPropertyName("totalMuseumValue")]
        public decimal TotalMuseumValue { get; set; }
    }
}
