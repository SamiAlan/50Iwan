using Iwan.Server.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Settings
{
    public class SlidersImagesSettingsBuilder : EntityBuilder<SlidersImagesSettings>
    {
        public override void Configure(EntityTypeBuilder<SlidersImagesSettings> builder)
        {
            // Table name
            builder.ToTable("SlidersImagesSettings");

            // Key
            builder.HasKey(r => r.Id);
        }
    }
}
