using Iwan.Server.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Products
{
    public class ProductStateBuilder : EntityBuilder<ProductState>
    {
        public override void Configure(EntityTypeBuilder<ProductState> builder)
        {
            builder.ToTable("ProductStates");
            builder.HasKey(c => c.Id);

            // Relations
            builder.HasOne(s => s.Product)
                .WithMany(p => p.States)
                .HasForeignKey(s => s.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
