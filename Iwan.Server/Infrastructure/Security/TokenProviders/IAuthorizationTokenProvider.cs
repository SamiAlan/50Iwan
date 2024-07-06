using Iwan.Server.Domain.Users;
using Iwan.Shared.Dtos.Accounts;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Infrastructure.Security.TokenProviders
{
    /// <summary>
    /// Represents an authorization token provider contract
    /// </summary>
    public interface IAuthorizationTokenProvider
    {
        /// <summary>
        /// Generates an <see cref="IAuthToken"/> for the passed user
        /// </summary>
        Task<IAuthToken> GenerateAuthTokenForUserAsync(AppUser user, CancellationToken token = default);
    }
}
