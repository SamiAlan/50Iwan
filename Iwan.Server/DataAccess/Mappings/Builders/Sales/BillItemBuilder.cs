using Iwan.Server.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Sales
{
    public class BillItemBuilder : EntityBuilder<BillItem>
    {
        public override void Configure(EntityTypeBuilder<BillItem> builder)
        {
            // Table jame
            builder.ToTable("BillItems");

            // Key
            builder.HasKey(b => b.Id);

            // Price
            builder.Property(r => r.Price).HasPrecision(15, 2);

            // Benefit Percent From Vendor
            builder.Property(r => r.BenefitPercentFromVendor).HasPrecision(5, 2);

            // Relations
            builder.HasOne(bi => bi.Bill)
                .WithMany(b => b.BillItems)
                .HasForeignKey(bi => bi.BillId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(bi => bi.Product)
                .WithMany()
                .HasForeignKey(bi => bi.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
