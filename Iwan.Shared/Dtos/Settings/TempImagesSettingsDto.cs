using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Settings
{
    public class TempImagesSettingsDto
    {
        [JsonPropertyName("tempImagesExpirationDays")]
        public int TempImagesExpirationDays { get; set; } = 7;

        [JsonPropertyName("delayInMinutes")]
        public int DelayInMinutes { get; set; }
    }
}
