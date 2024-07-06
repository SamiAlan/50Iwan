using Iwan.Server.Domain.Media;

namespace Iwan.Server.Domain.Products
{
    public class ProductMainImage : BaseEntity
    {
        /// <summary>
        /// Gets or sets the related product identifier
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the original image identifier
        /// </summary>
        public string OriginalImageId { get; set; }

        /// <summary>
        /// Gets or sets the medium image identifier
        /// </summary>
        public string MediumImageId { get; set; }

        /// <summary>
        /// Gets or sets the small image identifier
        /// </summary>
        public string SmallImageId { get; set; }

        /// <summary>
        /// Gets or sets the mobile image identifier
        /// </summary>
        public string MobileImageId { get; set; }

        /// <summary>
        /// Gets or sets the related product
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Gets or sets the related original image
        /// </summary>
        public virtual Image OriginalImage { get; set; }

        /// <summary>
        /// Gets or sets the related medium image
        /// </summary>
        public virtual Image MediumImage { get; set; }

        /// <summary>
        /// Gets or sets the related small image
        /// </summary>
        public virtual Image SmallImage { get; set; }

        /// <summary>
        /// Gets or sets the related mobile image
        /// </summary>
        public virtual Image MobileImage { get; set; }
    }
}
