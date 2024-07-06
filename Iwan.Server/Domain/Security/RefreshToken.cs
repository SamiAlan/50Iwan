using Iwan.Server.Domain.Users;
using System;

namespace Iwan.Server.Domain.Security
{
    /// <summary>
    /// Represents a refresh token business entity
    /// </summary>
    public class RefreshToken : BaseEntity
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public RefreshToken()
        {
            // Set the token to a random guid
            Token = System.Guid.NewGuid().ToString();
        }

        /// <summary>
        /// The refresh token string
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The related jwt identifier
        /// </summary>
        public string Jid { get; set; }

        /// <summary>
        /// Indicates whether the token is used or not
        /// </summary>
        public bool Used { get; set; }

        /// <summary>
        /// Indicates whether the token is invalidated or not
        /// </summary>
        public bool Invalidated { get; set; }

        /// <summary>
        /// The expire date of the token
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// The related user identifier
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the related user
        /// </summary>
        public virtual AppUser User { get; set; }
    }
}
