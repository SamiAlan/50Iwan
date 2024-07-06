using Iwan.Server.Domain.Media;

namespace Iwan.Server.Domain.Catelog
{
    public class CategoryImage : BaseEntity
    {
        /// <summary>
        /// Gets or sets the category identifier
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the original image identifier
        /// </summary>
        public string OriginalImageId { get; set; }

        /// <summary>
        /// Gets or sets the medium image identifier
        /// </summary>
        public string MediumImageId { get; set; }

        /// <summary>
        /// Gets or sets the mobile image identifier
        /// </summary>
        public string MobileImageId { get; set; }

        /// <summary>
        /// Gets or sets the category
        /// </summary>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Gets or sets the original image
        /// </summary>
        public virtual Image OriginalImage { get; set; }

        /// <summary>
        /// Gets or sets the medium image
        /// </summary>
        public virtual Image MediumImage { get; set; }

        /// <summary>
        /// Gets or sets the mobile image
        /// </summary>
        public virtual Image MobileImage { get; set; }
    }
}
