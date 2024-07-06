using Iwan.Server.Domain.Catelog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Catelog
{
    public class CategoryImageBuilder : EntityBuilder<CategoryImage>
    {
        public override void Configure(EntityTypeBuilder<CategoryImage> builder)
        {
            // Table name
            builder.ToTable("CategoryImages");

            // Key
            builder.HasKey(c => c.Id);

            // Relations
            builder.HasOne(c => c.Category)
                .WithOne(c => c.Image)
                .HasForeignKey<CategoryImage>(i => i.CategoryId)
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
