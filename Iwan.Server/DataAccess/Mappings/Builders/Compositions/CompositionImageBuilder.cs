using Iwan.Server.Domain.Compositions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Compositions
{
    public class CompositionImageBuilder : EntityBuilder<CompositionImage>
    {
        public override void Configure(EntityTypeBuilder<CompositionImage> builder)
        {
            builder.ToTable("CompositionImages");
            builder.HasKey(x => x.Id);

            // Relation
            builder.HasOne(i => i.Composition)
                .WithOne(c => c.Image)
                .HasForeignKey<CompositionImage>(i => i.CompositionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.OriginalImage)
                .WithMany()
                .HasForeignKey(c => c.OriginalImageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.MediumImage)
                .WithMany()
                .HasForeignKey(c => c.MediumImageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.MobileImage)
                .WithMany()
                .HasForeignKey(c => c.MobileImageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
