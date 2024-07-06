using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Pages
{
    public class ProductDetailsPageContentDto
    {
        [JsonPropertyName("colorPickingSection")]
        public ColorPickingSectionDto ColorPickingSection { get; set; } = new();
    }
}
