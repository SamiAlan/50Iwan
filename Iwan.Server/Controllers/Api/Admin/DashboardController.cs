using Iwan.Server.Queries.Dashboard.Admin;
using Iwan.Shared;
using Iwan.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Controllers.Api.Admin
{
    public class DashboardController : BaseAdminApiController
    {
        public DashboardController(IMediator mediator, IStringLocalizer<Localization> localizer) : base(mediator, localizer)
        {
        }

        [HttpGet]
        [Route(Routes.Api.Admin.Dashboard.BASE)]
        public async Task<IActionResult> GetDashboardData(CancellationToken cancellationToken = default)
        {
            var data = await _mediator.Send(new GetDashboardData.Request(), cancellationToken);

            return Ok(data);
        }
    }
}
