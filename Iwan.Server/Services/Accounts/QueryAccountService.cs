using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Users;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Accounts;
using Iwan.Shared.Extensions;
using Iwan.Shared.Options.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwan.Server.Extensions;
using Iwan.Shared.Constants;

namespace Iwan.Server.Services.Accounts
{
    [Injected(ServiceLifetime.Scoped, typeof(IQueryAccountService))]
    public class QueryAccountService : IQueryAccountService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ILoggedInUserProvider _loggedInUserProvider;

        public QueryAccountService(IUnitOfWork context,ILoggedInUserProvider loggedInUserProvider)
        {
            _unitOfWork = context;
            _loggedInUserProvider = loggedInUserProvider;
        }

        public async Task<AppUser> GetUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Users.FindAsync(new object[] { userId }, cancellationToken);
        }

        public async Task<UserDto> GetUserDetailsAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await GetUserAsync(userId, cancellationToken);

            if (user == null)
                throw new NotFoundException(Responses.Accounts.UserNotFound);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<PagedDto<UserDto>> GetUsersDetailsAsync(GetUsersOptions options, CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.Users.Where(u => u.Id != _loggedInUserProvider.UserId)
                .AsQueryable();

            if (!options.Email.IsNullOrEmptyOrWhiteSpaceSafe())
                query = query.Where(u => u.Email.ToLower().Contains(options.Email.ToLower()));

            if (!options.Name.IsNullOrEmptyOrWhiteSpaceSafe())
                query = query.Where(u => u.Name.ToLower().Contains(options.Name.ToLower()));

            var totalCount = await query.CountAsync(cancellationToken);
            var users = await query.Skip(options.CurrentPage * options.PageSize).Take(options.PageSize)
                .ToListAsync(cancellationToken);

            return users.AsPaged(options.CurrentPage, options.PageSize, totalCount, u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                CanDelete = _loggedInUserProvider.IsInRole(Roles.SuperAdmin)
            });
        }
    }
}
