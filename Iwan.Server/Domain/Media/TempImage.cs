using System;

namespace Iwan.Server.Domain.Media
{
    public class TempImage : BaseEntity
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
        /// Gets or sets the small version filename of the original image
        /// </summary>
        public string SmallVersionFileName { get; set; }

        /// <summary>
        /// Gets or sets the virtual path
        /// </summary>
        public string VirtualPath { get; set; }

        /// <summary>
        /// Gets or sets the date the temp image is expired
        /// </summary>
        public DateTime ExpirationDate { get; set; }
    }
}
