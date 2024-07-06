using Iwan.Server.Domain.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Media
{
    public class ImageBuilder : EntityBuilder<Image>
    {
        public override void Configure(EntityTypeBuilder<Image> builder)
        {
            // Table name
            builder.ToTable("Images");

            // Key
            builder.HasKey(i => i.Id);

            // Mime type
            builder.Property(i => i.MimeType).HasMaxLength(40).IsRequired();

            // Image Filename
            builder.Property(i => i.FileName).HasMaxLength(256).IsRequired();
        }
    }
}
