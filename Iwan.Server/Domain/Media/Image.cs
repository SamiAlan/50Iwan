namespace Iwan.Server.Domain.Media
{
    public class Image : BaseEntity
    {
        /// <summary>
        /// Gets or sets the image mime type
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the filename of the original image
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the virtual path
        /// </summary>
        public string VirtualPath { get; set; }
    }
}
