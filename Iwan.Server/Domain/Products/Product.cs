using Iwan.Server.Domain.Common;
using Iwan.Server.Domain.Vendors;
using System;
using System.Collections.Generic;

namespace Iwan.Server.Domain.Products
{
    public class Product : BaseEntity
    {
        /// <summary>
        /// Gets or sets the arabic name
        /// </summary>
        public string ArabicName { get; set; }

        /// <summary>
        /// Gets or sets the arabic description
        /// </summary>
        public string ArabicDescription { get; set; }

        /// <summary>
        /// Gets or sets the dimensions in  arabic
        /// </summary>
        public string DimensionsInArabic { get; set; }

        /// <summary>
        /// Gets or sets the english name
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// Gets or sets the english description
        /// </summary>
        public string EnglishDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the product has a main image
        /// </summary>
        public bool HasMainImage { get; set; }

        /// <summary>
        /// Gets or sets the dimensions in english
        /// </summary>
        public string DimensionsInEnglish { get; set; }

        /// <summary>
        /// Gets or sets the color code
        /// </summary>
        public string ColorCode { get; set; }

        /// <summary>
        /// Gets or sets the number
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Get or sets the 
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the arabic maker name
        /// </summary>
        public string MakerArabicName { get; set; }

        /// <summary>
        /// Gets or sets the english maker name
        /// </summary>
        public string MakerEnglishName { get; set; }

        /// <summary>
        /// Gets or sets the stock quantity
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// Gets or sets the price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that the product should be visible or not
        /// </summary>
        public bool IsVisible { get; set; }

        public DateTime LastResizeDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the color type identifier
        /// </summary>
        public int ColorTypeId { get; set; }

        /// <summary>
        /// Gets or sets the color type
        /// </summary>
        public ColorType ColorType
        {
            get => (ColorType)ColorTypeId;
            set => ColorTypeId = (int)value;
        }

        /// <summary>
        /// Gets or sets the vendor identifier
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// Gets or sets the vender
        /// </summary>
        public virtual Vendor Vendor { get; set; }

        /// <summary>
        /// Gets or sets the product's main image
        /// </summary>
        public virtual ProductMainImage MainImage { get; set; }

        /// <summary>
        /// Gets or sets the product categories
        /// </summary>
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }

        /// <summary>
        /// Gets or sets the product images
        /// </summary>
        public virtual ICollection<ProductImage> Images { get; set; }

        /// <summary>
        /// Gets or sets the product states
        /// </summary>
        public virtual ICollection<ProductState> States { get; set; }
    }
}
