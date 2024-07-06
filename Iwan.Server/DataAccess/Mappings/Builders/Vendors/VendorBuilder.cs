using Iwan.Server.Domain.Vendors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Vendors
{
    public class VendorBuilder : EntityBuilder<Vendor>
    {
        public override void Configure(EntityTypeBuilder<Vendor> builder)
        {
            // Table name
            builder.ToTable("Vendors");

            // Key
            builder.HasKey(a => a.Id);

            // Benefit Percent
            builder.Property(v => v.BenefitPercent).HasPrecision(5, 2);
            
            // Relations
            builder.HasOne(v => v.Address)
                .WithMany()
                .HasForeignKey(v => v.AddressId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
