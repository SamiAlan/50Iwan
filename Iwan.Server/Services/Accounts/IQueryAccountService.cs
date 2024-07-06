using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Accounts;
using Iwan.Shared.Options.Accounts;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Accounts
{
    public interface IQueryAccountService
    {
        Task<UserDto> GetUserDetailsAsync(string userId, CancellationToken cancellationToken = default);
        Task<PagedDto<UserDto>> GetUsersDetailsAsync(GetUsersOptions options, CancellationToken cancellationToken = default);
    }
}
