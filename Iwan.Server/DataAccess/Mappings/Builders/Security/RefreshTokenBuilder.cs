using Iwan.Server.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Security
{
    public class RefreshTokenBuilder : EntityBuilder<RefreshToken>
    {
        public override void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            // Table name
            builder.ToTable("RefreshTokens");

            // Key
            builder.HasKey(r => r.Id);

            // Token
            builder.Property(r => r.Token).HasMaxLength(128).IsRequired();

            // Jid
            builder.Property(r => r.Jid).HasMaxLength(128).IsRequired();

            // Relations
            builder.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
