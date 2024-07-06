using Iwan.Server.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Settings
{
    public class ProductsImagesSettingsBuilder : EntityBuilder<ProductsImagesSettings>
    {
        public override void Configure(EntityTypeBuilder<ProductsImagesSettings> builder)
        {
            // Table name
            builder.ToTable("ProductsImagesSettings");

            // Key
            builder.HasKey(r => r.Id);
        }
    }
}
