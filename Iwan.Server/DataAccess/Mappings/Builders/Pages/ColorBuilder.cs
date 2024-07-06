using Iwan.Server.Domain.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Pages
{
    public class ColorBuilder : EntityBuilder<Color>
    {
        public override void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.ToTable("Colors");
            builder.HasKey(c => c.Id);

            // Relations
            builder.HasOne(c => c.Section)
                .WithMany(s => s.Colors)
                .HasForeignKey(c => c.SectionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
