using Iwan.Server.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Products
{
    public class ProductImageBuilder : EntityBuilder<ProductImage>
    {
        public override void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            // Table name
            builder.ToTable("ProductImages");

            // Key
            builder.HasKey(p => p.Id);

            // Relations
            builder.HasOne(pp => pp.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pp => pp.ProductId)
                .IsRequired()
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
