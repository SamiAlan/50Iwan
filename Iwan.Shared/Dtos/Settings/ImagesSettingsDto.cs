using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Settings
{
    public class ImagesSettingsDto
    {
        [JsonPropertyName("productMediumImageWidth")]
        public int ProductMediumImageWidth { get; set; }

        [JsonPropertyName("productMediumImageHeight")]
        public int ProductMediumImageHeight { get; set; }

        [JsonPropertyName("productSmallImageWidth")]
        public int ProductSmallImageWidth { get; set; }

        [JsonPropertyName("productSmallImageHeight")]
        public int ProductSmallImageHeight { get; set; }

        [JsonPropertyName("productMobileImageWidth")]
        public int ProductMobileImageWidth { get; set; }

        [JsonPropertyName("productMobileImageHeight")]
        public int ProductMobileImageHeight { get; set; }


        [JsonPropertyName("sliderImageMediumWidth")]
        public int SliderImageMediumWidth { get; set; }

        [JsonPropertyName("sliderImageMediumHeight")]
        public int SliderImageMediumHeight { get; set; }

        [JsonPropertyName("sliderImageMobileWidth")]
        public int SliderImageMobileWidth { get; set; }

        [JsonPropertyName("sliderImageMobileHeight")]
        public int SliderImageMobileHeight { get; set; }


        [JsonPropertyName("categoryMediumImageWidth")]
        public int CategoryMediumImageWidth { get; set; }

        [JsonPropertyName("categoryMediumImageHeight")]
        public int CategoryMediumImageHeight { get; set; }

        [JsonPropertyName("categoryMobileImageWidth")]
        public int CategoryMobileImageWidth { get; set; }

        [JsonPropertyName("categoryMobileImageHeight")]
        public int CategoryMobileImageHeight { get; set; }


        [JsonPropertyName("compositionMediumImageWidth")]
        public int CompositionMediumImageWidth { get; set; }

        [JsonPropertyName("compositionMediumImageHeight")]
        public int CompositionMediumImageHeight { get; set; }

        [JsonPropertyName("compositionMobileImageWidth")]
        public int CompositionMobileImageWidth { get; set; }

        [JsonPropertyName("compositionMobileImageHeight")]
        public int CompositionMobileImageHeight { get; set; }


        [JsonPropertyName("aboutUsSectionMediumImageWidth")]
        public int AboutUsSectionMediumImageWidth { get; set; }

        [JsonPropertyName("aboutUsSectionMediumImageHeight")]
        public int AboutUsSectionMediumImageHeight { get; set; }

        [JsonPropertyName("aboutUsSectionMobileImageWidth")]
        public int AboutUsSectionMobileImageWidth { get; set; }

        [JsonPropertyName("aboutUsSectionMobileImageHeight")]
        public int AboutUsSectionMobileImageHeight { get; set; }
    }
}
