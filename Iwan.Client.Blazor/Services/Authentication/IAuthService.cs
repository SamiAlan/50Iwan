using Iwan.Shared.Common;
using Iwan.Shared.Dtos.Accounts;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Authentication
{
    public interface IAuthService
    {
        Task<ApiResponse<JwtAuthToken>> LoginAsync(LoginDto loginCredentials, CancellationToken cancellationToken = default);
        Task<ApiResponse<JwtAuthToken>> RefreshTokenAsync(RefreshUserTokenDto refreshToken, CancellationToken cancellationToken = default);
    }
}
