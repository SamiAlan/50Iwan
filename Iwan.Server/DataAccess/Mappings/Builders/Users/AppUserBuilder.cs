using Iwan.Server.Domain.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Users
{
    public class AppUserBuilder : EntityBuilder<AppUser>
    {
        public override void Configure(EntityTypeBuilder<AppUser> builder)
        {
            // Key
            builder.HasKey(u => u.Id);

            // Name
            builder.Property(a => a.Name).IsRequired();
        }
    }
}
