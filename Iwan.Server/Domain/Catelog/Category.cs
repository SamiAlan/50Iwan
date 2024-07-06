using Iwan.Server.Domain.Common;
using System.Collections.Generic;

namespace Iwan.Server.Domain.Catelog
{
    public class Category : BaseEntity
    {
        /// <summary>
        /// Gets or sets the arabic name
        /// </summary>
        public string ArabicName { get; set; }

        /// <summary>
        /// Gets or sets the arabic name
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this category is a sub-category
        /// Used for query optimization where sub-categories could be fetched faster
        /// </summary>
        public bool IsSubCategory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this category is visible to the client
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the parent category identifier (if exists)
        /// </summary>
        public string ParentCategoryId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the category has attached sub-categories
        /// Used for query optimization in order to be able to retrieve sub-categories only when they already exist
        /// </summary>
        public bool HasSubCategories { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the category has attached products
        /// Used for query optimization in order to be able to retrieve sub-categories only 
        /// when there are attached products to it
        /// </summary>
        public bool HasRelatedProducts { get; set; }

        /// <summary>
        /// Gets or sets the color code
        /// </summary>
        public string ColorCode { get; set; }

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
        /// Gets or sets the parent category (if exists)
        /// </summary>
        public virtual Category ParentCategory { get; set; }

        /// <summary>
        /// Gets or sets the image
        /// </summary>
        public virtual CategoryImage Image { get; set; }

        /// <summary>
        /// Gets or sets the sub-categories collection
        /// </summary>
        public virtual ICollection<Category> SubCategories { get; set; }
    }
}
