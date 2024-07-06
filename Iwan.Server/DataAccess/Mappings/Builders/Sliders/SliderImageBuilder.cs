using Iwan.Server.Domain.Sliders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Sliders
{
    public class SliderImageBuilder : EntityBuilder<SliderImage>
    {
        public override void Configure(EntityTypeBuilder<SliderImage> builder)
        {
            // Table name
            builder.ToTable("SliderImages");

            // Key
            builder.HasKey(s => s.Id);

            // Relations
            builder.HasOne(si => si.OriginalImage)
                .WithMany()
                .HasForeignKey(si => si.OriginalImageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(si => si.MediumImage)
                .WithMany()
                .HasForeignKey(si => si.MediumImageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(si => si.MobileImage)
                .WithMany()
                .HasForeignKey(si => si.MobileImageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
