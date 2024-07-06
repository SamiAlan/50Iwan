using Iwan.Server.Domain.Catelog;

namespace Iwan.Server.Domain.Products
{
    public class ProductCategory : BaseEntity
    {
        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the category identifier
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the related product
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Gets or sets the related category
        /// </summary>
        public virtual Category Category { get; set; }
    }
}
