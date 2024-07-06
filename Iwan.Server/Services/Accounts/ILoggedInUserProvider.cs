using Iwan.Server.Domain.Users;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Accounts
{
    /// <summary>
    /// Represents a service contract for the logged in user
    /// </summary>
    public interface ILoggedInUserProvider
    {
        /// <summary>
        /// The current user identifier
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// The current user instance
        /// </summary>
        /// <returns></returns>
        Task<AppUser> GetCurrentUser();

        /// <summary>
        /// Checks if the user in the passed role
        /// </summary>
        /// <param name="role">The role to check</param>
        /// <returns></returns>
        bool IsInRole(string role);

        /// <summary>
        /// The claims principal of the current user
        /// </summary>
        ClaimsPrincipal ClaimsPrincipal { get; }

        /// <summary>
        /// The culture of the currenly logged in user
        /// </summary>
        public CultureInfo Culture { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the user's culture is arabic or not
        /// </summary>
        public bool IsCultureArabic { get; }
    }
}
