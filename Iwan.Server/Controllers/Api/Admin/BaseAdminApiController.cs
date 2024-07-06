using Iwan.Shared;
using Iwan.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

namespace Iwan.Server.Controllers.Api.Admin
{
    [Authorize(Roles = $"{Roles.Admin}, {Roles.SuperAdmin}")]
    public class BaseAdminApiController : BaseApiController
    {
        public BaseAdminApiController(IMediator mediator, IStringLocalizer<Localization> localizer) : base(mediator, localizer) { }
    }
}
