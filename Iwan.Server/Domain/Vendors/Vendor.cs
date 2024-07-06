using Iwan.Server.Domain.Common;
using Iwan.Server.Domain.Products;
using System.Collections.Generic;

namespace Iwan.Server.Domain.Vendors
{
    public class Vendor : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show vender products or not
        /// </summary>
        public bool ShowOwnProducts { get; set; }

        /// <summary>
        /// Gets or sets the benefit percent to be got from selling vendor's products
        /// </summary>
        public decimal BenefitPercent { get; set; }

        /// <summary>
        /// Gets or sets the address identifier
        /// </summary>
        public string AddressId { get; set; }

        /// <summary>
        /// Gets or sets the address
        /// </summary>
        public virtual Address Address { get; set; }

        /// <summary>
        /// Gets or sets the products
        /// </summary>
        public virtual IList<Product> Products { get; set; }
    }
}
