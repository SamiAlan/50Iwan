using Iwan.Shared.Extensions;
using System.Collections.Generic;

namespace Iwan.Shared.Constants
{
    public static class Roles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";

        public static bool Contains(string roleName)
        {
            if (roleName.EqualsIgnoreCase(Admin)) return true;
            else if (roleName.EqualsIgnoreCase(SuperAdmin)) return true;
            return false;
        }

        public static IEnumerable<string> All() => new List<string> { Admin, SuperAdmin };
    }
}
