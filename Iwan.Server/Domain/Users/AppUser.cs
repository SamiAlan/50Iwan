using Microsoft.AspNetCore.Identity;

namespace Iwan.Server.Domain.Users
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
