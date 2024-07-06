using Iwan.Server.Commands.Accounts;
using Iwan.Server.Commands.Accounts.Admin;
using Iwan.Server.Events.Accounts;
using Iwan.Server.Queries.Accounts.Admin;
using Iwan.Server.Services.Accounts;
using Iwan.Shared;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Accounts;
using Iwan.Shared.Options.Accounts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Controllers.Api.Admin
{
    public class AccountsController : BaseAdminApiController
    {
        protected readonly ILoggedInUserProvider _loggedInUserProvider;

        public AccountsController(IMediator mediator, IStringLocalizer<Localization> stringLocalizer,
            ILoggedInUserProvider loggedInUserProvider)
            : base(mediator, stringLocalizer)
        {
            _loggedInUserProvider = loggedInUserProvider;
        }

        [HttpGet]
        [Route(Routes.Api.Admin.Accounts.BASE)]
        public async Task<IActionResult> Get([FromQuery]GetUsersOptions options, CancellationToken cancellationToken = default)
        {
            var users = await _mediator.Send(new GetUsers.Request(options), cancellationToken);

            return Ok(users);
        }

        [HttpGet]
        [Route(Routes.Api.Admin.Accounts.Profile)]
        public async Task<IActionResult> GetProfile(CancellationToken cancellationToken = default)
        {
            var user = await _mediator.Send(new GetCurrentUser.Request(), cancellationToken);

            return Ok(user);
        }

        [HttpPost]
        [Route(Routes.Api.Admin.Accounts.Login)]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto credentials, CancellationToken token = default)
        {
            var authToken = await _mediator.Send(new Login.Request(credentials), token);

            await _mediator.Publish(new UserLoggedInEvent(_loggedInUserProvider.UserId), token);

            return Ok(authToken);
        }

        [HttpPost]
        [Route(Routes.Api.Admin.Accounts.AddAdminUser)]
        [Authorize(Roles = Roles.SuperAdmin)]
        public async Task<IActionResult> AddUser(AddUserDto adminUser, CancellationToken token = default)
        {
            var user = await _mediator.Send(new AddUser.Request(adminUser), token);

            await _mediator.Publish(new AppUserAddedEvent(user.Id), token);

            return base.Ok((object)user);
        }

        [HttpPost]
        [Route(Routes.Api.Admin.Accounts.RefreshToken)]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(RefreshUserTokenDto refreshRequest, CancellationToken token = default)
        {
            // Send a refresh token command
            var authToken = await _mediator.Send(new RefreshToken.Request(refreshRequest), token);

            await _mediator.Publish(new UserRefreshedHisTokenEvent(_loggedInUserProvider.UserId), token);

            // Return an ok result with the resulting data
            return Ok(authToken);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Accounts.Profile)]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto profile, CancellationToken token = default)
        {
            // Send an update profile command
            var user = await _mediator.Send(new UpdateProfile.Request(profile), token);

            await _mediator.Publish(new UserUpdatedProfileEvent(_loggedInUserProvider.UserId), token);

            // Return the resulting data
            return Ok(user);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Accounts.ChangePassword)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto credentials, CancellationToken token = default)
        {
            // Send a change password command
            await _mediator.Send(new ChangePassword.Request(credentials), token);

            await _mediator.Publish(new UserChangedPasswordEvent(_loggedInUserProvider.UserId), token);

            // Return the resulting data
            return Ok();
        }

        [HttpDelete]
        [Route(Routes.Api.Admin.Accounts.Delete)]
        [Authorize(Roles = Roles.SuperAdmin)]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteUser.Request(id), cancellationToken);

            await _mediator.Publish(new UserDeletedEvent(id), cancellationToken);

            return Ok();
        }
    }
}
