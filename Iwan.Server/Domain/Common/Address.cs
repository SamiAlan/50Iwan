namespace Iwan.Server.Domain.Common
{
    public class Address : BaseEntity
    {
        /// <summary>
        /// Gets or sets the city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the company
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the first address
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the second address
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }
    }
}
