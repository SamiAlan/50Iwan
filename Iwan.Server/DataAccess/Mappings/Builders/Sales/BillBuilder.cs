using Iwan.Server.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Sales
{
    public class BillBuilder : EntityBuilder<Bill>
    {
        public override void Configure(EntityTypeBuilder<Bill> builder)
        {
            // Table jame
            builder.ToTable("Bills");

            // Key
            builder.HasKey(b => b.Id);

            // Customer Name
            builder.Property(b => b.CustomerName).HasMaxLength(128);

            // Customer Phone
            builder.Property(b => b.CustomerPhone).HasMaxLength(64);

            // Total
            builder.Property(r => r.Total).HasPrecision(15, 2);
        }
    }
}
