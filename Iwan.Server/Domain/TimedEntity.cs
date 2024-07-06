using System;

namespace Iwan.Server.Domain
{
    public abstract class TimedEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the universal date the entity has been created
        /// </summary>
        public DateTime CreatedDateUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the universal date the entity has lastly been updated
        /// </summary>
        public DateTime UpdatedDateUtc { get; set; } = DateTime.UtcNow;
    }
}
