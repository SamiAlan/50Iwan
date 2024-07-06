using System.Collections.Generic;

namespace Iwan.Server.Domain.Sales
{
    public class Bill : TimedEntity
    {
        /// <summary>
        /// Gets or sets the bill number
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the total price of the bill
        /// </summary>
        public decimal Total { get; set; }
        
        /// <summary>
        /// Gets or sets the bill's customer name 
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the bill's customer phone number
        /// </summary>
        public string CustomerPhone { get; set; }

        /// <summary>
        /// Gets or sets the related bill items
        /// </summary>
        public virtual ICollection<BillItem> BillItems { get; set; }
    }
}
