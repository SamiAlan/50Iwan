﻿using Microsoft.EntityFrameworkCore;

namespace Iwan.Server.DataAccess.Mappings
{
    /// <summary>
    /// Represents an entity builder contract for building mappings of business models
    /// </summary>
    public interface IEntityBuilder
    {
        /// <summary>
        /// Applies the configurations for a business model class
        /// </summary>
        void ApplyConfiguration(ModelBuilder builder);
    }
}
