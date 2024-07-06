using Iwan.Server.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Settings
{
    public class AboutUsSectionImagesSettingsBuilder : EntityBuilder<AboutUsSectionImagesSettings>
    {
        public override void Configure(EntityTypeBuilder<AboutUsSectionImagesSettings> builder)
        {
            builder.ToTable("AboutUsSectionImagesSettings");
            builder.HasKey(e => e.Id);
        }
    }
}
