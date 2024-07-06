using Iwan.Server.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Products
{
    public class ProductBuilder : EntityBuilder<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            // Table name
            builder.ToTable("Products");

            // Key
            builder.HasKey(p => p.Id);

            // Price
            builder.Property(p => p.Price).HasPrecision(15, 2);

            // Color type
            builder.Ignore(p => p.ColorType);

            // Relations
            builder.HasOne(p => p.Vendor)
                .WithMany(v => v.Products)
                .HasForeignKey(p => p.VendorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
