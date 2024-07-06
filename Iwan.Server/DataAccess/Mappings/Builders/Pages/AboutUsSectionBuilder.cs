using Iwan.Server.Domain.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Pages
{
    public class AboutUsSectionBuilder : EntityBuilder<AboutUsSection>
    {
        public override void Configure(EntityTypeBuilder<AboutUsSection> builder)
        {
            builder.ToTable("AboutUsSections");
            builder.HasKey(h => h.Id);
        }
    }
}
