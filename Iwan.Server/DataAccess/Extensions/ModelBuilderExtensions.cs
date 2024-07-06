using Iwan.Shared.Constants;
using Iwan.Server.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Iwan.Server.DataAccess.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void SeedDefaultUsers(this ModelBuilder builder)
        {
            IdentityRole superAdminRole = null;

            foreach (var role in Roles.All())
            {
                var addedRole = new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = role,
                    NormalizedName = role.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };

                if (addedRole.Name == Roles.SuperAdmin) superAdminRole = addedRole;

                // Add the role
                builder.Entity<IdentityRole>().HasData(addedRole);
            }

            // Create the admin user 
            var adminUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Admin",
                UserName = "admin5245",
                NormalizedUserName = "ADMIN@IWAN.COM",
                NormalizedEmail = "ADMIN@IWAN.COM",
                Email = "admin@iwan.com",
                EmailConfirmed = true
            };

            // Add the hashed password
            adminUser.PasswordHash = new PasswordHasher<AppUser>().HashPassword(adminUser, "Admin.123");

            // Add the user
            builder.Entity<AppUser>().HasData(adminUser);

            // Add the UserRole entry
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = adminUser.Id,
                RoleId = superAdminRole.Id
            });
        }
    }
}
