using Iwan.Server.Domain.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Pages
{
    public class ContactUsSectionBuilder : EntityBuilder<ContactUsSection>
    {
        public override void Configure(EntityTypeBuilder<ContactUsSection> builder)
        {
            builder.ToTable("ContactUsSections");
            builder.HasKey(h => h.Id);
        }
    }
}
