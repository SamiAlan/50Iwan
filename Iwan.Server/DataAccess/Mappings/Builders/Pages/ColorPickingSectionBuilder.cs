using Iwan.Server.Domain.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Pages
{
    public class ColorPickingSectionBuilder : EntityBuilder<ColorPickingSection>
    {
        public override void Configure(EntityTypeBuilder<ColorPickingSection> builder)
        {
            builder.ToTable("ColorPickingSections");
            builder.HasKey(s => s.Id);
        }
    }
}
