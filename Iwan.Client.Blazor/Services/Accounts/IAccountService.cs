using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Accounts;
using Iwan.Shared.Options.Accounts;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Accounts
{
    public interface IAccountService
    {
        Task<PagedDto<UserDto>> GetUsersAsync(GetUsersOptions options, CancellationToken cancellationToken = default);
        Task<UserDto> GetCurrentUserAsync(CancellationToken cancellationToken = default);
        Task<UserDto> AddUserAsync(AddUserDto adminUser, CancellationToken cancellationToken = default);
        Task<UserDto> UpdateProfileAsync(UpdateProfileDto profile, CancellationToken cancellationToken = default);
        Task ChangePasswordAsync(ChangePasswordDto password, CancellationToken cancellationToken = default);
        Task DeleteUserAsync(string id, CancellationToken cancellationToken = default);
    }
}
