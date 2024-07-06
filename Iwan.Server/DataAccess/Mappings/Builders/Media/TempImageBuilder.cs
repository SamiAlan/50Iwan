using Iwan.Server.Domain.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Media
{
    public class TempImageBuilder : EntityBuilder<TempImage>
    {
        public override void Configure(EntityTypeBuilder<TempImage> builder)
        {
            // Table name
            builder.ToTable("TempImages");

            // Key
            builder.HasKey(t => t.Id);

            // Filename
            builder.Property(t => t.FileName).HasMaxLength(128).IsRequired();

            // Filename
            builder.Property(t => t.SmallVersionFileName).HasMaxLength(128).IsRequired();

            // Mimetype
            builder.Property(t => t.MimeType).HasMaxLength(64).IsRequired();
        }
    }
}
