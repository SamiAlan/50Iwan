using Iwan.Server.Domain.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Pages
{
    public class InteriorDesignSectionImageBuilder : EntityBuilder<InteriorDesignSectionImage>
    {
        public override void Configure(EntityTypeBuilder<InteriorDesignSectionImage> builder)
        {
            builder.ToTable("InteriorDesignSectionImages");
            builder.HasKey(h => h.Id);

            // Relations
            builder.HasOne(e => e.InteriorDesignSection)
                .WithOne(e => e.Image)
                .HasForeignKey<InteriorDesignSectionImage>(e => e.InteriorDesignSectionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
