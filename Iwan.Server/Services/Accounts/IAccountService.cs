using Iwan.Server.Domain.Users;
using Iwan.Shared.Dtos.Accounts;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Accounts
{
    public interface IAccountService
    {
        Task<AppUser> AddUserAsync(AddUserDto userToAdd, string roleName, CancellationToken cancellationToken = default);

        Task<IAuthToken> LoginAsync(LoginDto loginCredentials, CancellationToken cancellationToken = default);

        Task<IAuthToken> RefreshTokenAsync(RefreshUserTokenDto refreshUserToken, CancellationToken cancellationToken = default);

        Task UpdateProfileAsync(UpdateProfileDto profile, CancellationToken cancellationToken = default);

        Task ChangePasswordAsync(ChangePasswordDto changePasswordDto, CancellationToken cancellationToken = default);

        Task DeleteUserAsync(string id, CancellationToken cancellationToken = default);
    }
}
