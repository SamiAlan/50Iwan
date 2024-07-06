using Iwan.Shared.Constants;
using System.Collections.Generic;

namespace Iwan.Server.Constants
{
    public static class Policies
    {
        public static class HangfireDashboard
        {
            public static string Name = "HangfireDashboard";
            public static IList<string> RequiredRoles = new List<string>
            {
                Roles.Admin, Roles.SuperAdmin
            };
        }
    }
}
