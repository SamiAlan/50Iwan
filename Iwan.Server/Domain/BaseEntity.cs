using System;

namespace Iwan.Server.Domain
{
    /// <summary>
    /// Represents the base class for entities
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
    }
}
