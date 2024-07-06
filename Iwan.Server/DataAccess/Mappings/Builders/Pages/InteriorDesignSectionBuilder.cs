using Iwan.Server.Domain.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Pages
{
    public class InteriorDesignSectionBuilder : EntityBuilder<InteriorDesignSection>
    {
        public override void Configure(EntityTypeBuilder<InteriorDesignSection> builder)
        {
            builder.ToTable("InteriorDesignSections");
            builder.HasKey(h => h.Id);
        }
    }
}
