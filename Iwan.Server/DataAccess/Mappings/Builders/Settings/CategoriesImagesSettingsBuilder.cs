using Iwan.Server.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Settings
{

    public class CategoriesImagesSettingsBuilder : EntityBuilder<CategoriesImagesSettings>
    {
        public override void Configure(EntityTypeBuilder<CategoriesImagesSettings> builder)
        {
            // Table name
            builder.ToTable("CategoriesImagesSettings");

            // Key
            builder.HasKey(r => r.Id);
        }
    }
}
