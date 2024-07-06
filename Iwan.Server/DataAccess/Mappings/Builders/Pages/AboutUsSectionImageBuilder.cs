using Iwan.Server.Domain.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Pages
{
    public class AboutUsSectionImageBuilder : EntityBuilder<AboutUsSectionImage>
    {
        public override void Configure(EntityTypeBuilder<AboutUsSectionImage> builder)
        {
            builder.ToTable("AboutUsSectionImages");
            builder.HasKey(h => h.Id);

            // Relations
            builder.HasOne(e => e.AboutUsSection)
                .WithMany(e => e.Images)
                .HasForeignKey(e => e.AboutUsSectionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
