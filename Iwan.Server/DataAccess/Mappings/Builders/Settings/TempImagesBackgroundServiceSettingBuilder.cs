using Iwan.Server.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Settings
{
    public class TempImagesBackgroundServiceSettingBuilder : EntityBuilder<TempImagesSettings>
    {
        public override void Configure(EntityTypeBuilder<TempImagesSettings> builder)
        {
            // Table name
            builder.ToTable("TempImagesBackgroundServiceSettings");

            // Key
            builder.HasKey(t => t.Id);
        }
    }
}
