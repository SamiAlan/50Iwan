using Iwan.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Iwan.Server.Controllers.Api
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly IStringLocalizer<Localization> _stringLocalizer;

        public BaseApiController(IMediator mediator, IStringLocalizer<Localization> localizer)
        {
            _mediator = mediator;
            _stringLocalizer = localizer;
        }
    }
}
