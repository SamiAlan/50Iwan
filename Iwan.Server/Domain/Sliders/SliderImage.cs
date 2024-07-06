using Iwan.Server.Domain.Media;
using Iwan.Shared.Options.SliderImages;

namespace Iwan.Server.Domain.Sliders
{
    public class SliderImage : BaseEntity
    {
        /// <summary>
        /// Gets or sets the order
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the original image identifier
        /// </summary>
        public string OriginalImageId { get; set; }

        /// <summary>
        /// Gets or sets the medium image identifier
        /// </summary>
        public string MediumImageId { get; set; }

        /// <summary>
        /// Gets or sets the mobile-sized image identifier
        /// </summary>
        public string MobileImageId { get; set; }

        /// <summary>
        /// Gets or sets the original image
        /// </summary>
        public virtual Image OriginalImage { get; set; }

        /// <summary>
        /// Gets or sets the medium image
        /// </summary>
        public virtual Image MediumImage { get; set; }

        /// <summary>
        /// Gets or sets the mobile-sized image
        /// </summary>
        public virtual Image MobileImage { get; set; }
    }
}
