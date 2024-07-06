using Iwan.Server.Domain.Users;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Shared.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;

namespace Iwan.Server.Services.Accounts
{
    /// <summary>
    /// Represents a provider class for the logged in user
    /// </summary>
    [Injected(ServiceLifetime.Scoped, typeof(ILoggedInUserProvider))]
    public class LoggedInUserProvider : ILoggedInUserProvider
    {
        /// <summary>
        /// The http accessor instance
        /// </summary>
        protected readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The manager used to do operations on the <see cref="AppUser"/>
        /// </summary>
        protected readonly UserManager<AppUser> _userManager;

        private AppUser _currentUser;

        /// <summary>
        /// Constructs a new instance of the <see cref="LoggedInUserProvider"/> class using the passed parameters
        /// </summary>
        public LoggedInUserProvider(IHttpContextAccessor httpContextAccessor,
            UserManager<AppUser> userManager)
        {
            // Set the http accesssor
            _httpContextAccessor = httpContextAccessor;

            // Set the user manager
            _userManager = userManager;

            // Get the http context
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                // Get the user identifier if it is there
                UserId = httpContext.User
                    ?.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                // Get the claims principal if it is there
                ClaimsPrincipal = httpContext.User;

                Culture = httpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture;
                
                IsCultureArabic = Culture?.IsArabic() ?? false;
            }
        }

        /// <summary>
        /// The current user identifier
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// The current user claims principal
        /// </summary>
        public ClaimsPrincipal ClaimsPrincipal { get; }

        /// <summary>
        /// Gets or sets the current user's culture
        /// </summary>
        public CultureInfo Culture { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the user's culture is arabic or not
        /// </summary>
        public bool IsCultureArabic { get; }

        /// <summary>
        /// Returns the current user instance
        /// </summary>
        public async Task<AppUser> GetCurrentUser()
        {
            if (string.IsNullOrEmpty(UserId)) return null;

            if (_currentUser == null) _currentUser = await _userManager.FindByIdAsync(UserId);

            return _currentUser;
        }

        /// <summary>
        /// Checks if the user in the passed role
        /// </summary>
        /// <param name="role">The role to check</param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            return ClaimsPrincipal.IsInRole(role);
        }
    }
}
