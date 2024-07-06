using Iwan.Server.Domain.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Pages
{
    public class ServicesSectionBuilder : EntityBuilder<ServicesSection>
    {
        public override void Configure(EntityTypeBuilder<ServicesSection> builder)
        {
            builder.ToTable("ServicesSections");
            builder.HasKey(h => h.Id);
        }
    }
}
