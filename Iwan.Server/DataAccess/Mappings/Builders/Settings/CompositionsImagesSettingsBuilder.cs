using Iwan.Server.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Settings
{
    public class CompositionsImagesSettingsBuilder : EntityBuilder<CompositionsImagesSettings>
    {
        public override void Configure(EntityTypeBuilder<CompositionsImagesSettings> builder)
        {
            // Table name
            builder.ToTable("CompositionsImagesSettings");

            // Key
            builder.HasKey(r => r.Id);
        }
    }
}
