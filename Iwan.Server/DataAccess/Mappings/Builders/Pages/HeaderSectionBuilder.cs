using Iwan.Server.Domain.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Pages
{
    public class HeaderSectionBuilder : EntityBuilder<HeaderSection>
    {
        public override void Configure(EntityTypeBuilder<HeaderSection> builder)
        {
            builder.ToTable("HeaderSections");
            builder.HasKey(h => h.Id);
        }
    }
}
