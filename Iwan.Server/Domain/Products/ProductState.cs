namespace Iwan.Server.Domain.Products
{
    public class ProductState : BaseEntity
    {
        /// <summary>
        /// Gets or sets the arabic name
        /// </summary>
        public string ArabicName { get; set; }

        /// <summary>
        /// Gets or sets the english name
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product
        /// </summary>
        public virtual Product Product { get; set; }
    }
}
