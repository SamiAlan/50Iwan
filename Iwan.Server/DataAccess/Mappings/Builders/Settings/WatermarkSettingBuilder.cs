using Iwan.Server.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Settings
{
    public class WatermarkSettingBuilder : EntityBuilder<WatermarkSettings>
    {
        public override void Configure(EntityTypeBuilder<WatermarkSettings> builder)
        {
            builder.ToTable("WatermarkSettings");
            builder.HasKey(s => s.Id);

            // Relations
            builder.HasOne(s => s.WatermarkImage)
                .WithMany()
                .HasForeignKey(s => s.WatermarkImageId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
