using Iwan.Server.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Products
{
    public class ProductMainImageBuilder : EntityBuilder<ProductMainImage>
    {
        public override void Configure(EntityTypeBuilder<ProductMainImage> builder)
        {
            builder.ToTable("ProductMainImages");
            builder.HasKey(i => i.Id);

            builder.HasOne(i => i.Product)
                .WithOne(p => p.MainImage)
                .HasForeignKey<ProductMainImage>(i => i.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(pp => pp.OriginalImage)
                .WithMany()
                .HasForeignKey(pp => pp.OriginalImageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(pp => pp.MediumImage)
                .WithMany()
                .HasForeignKey(pp => pp.MediumImageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(pp => pp.SmallImage)
                .WithMany()
                .HasForeignKey(pp => pp.SmallImageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(pp => pp.MobileImage)
                .WithMany()
                .HasForeignKey(pp => pp.MobileImageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
