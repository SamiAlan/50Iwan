using Iwan.Server.Domain.Products;

namespace Iwan.Server.Domain.Sales
{
    public class BillItem : TimedEntity
    {
        /// <summary>
        /// Gets or sets the price of the bill item
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the related bill identifier
        /// </summary>
        public string BillId { get; set; }

        /// <summary>
        /// Gets or sets the related product identifier
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the benefit percent from the vendor
        /// </summary>
        public decimal BenefitPercentFromVendor { get; set; }

        /// <summary>
        /// Gets or sets the related bill
        /// </summary>
        public virtual Bill Bill { get; set; }

        /// <summary>
        /// Gets or sets the related product
        /// </summary>
        public virtual Product Product { get; set; }
    }
}
