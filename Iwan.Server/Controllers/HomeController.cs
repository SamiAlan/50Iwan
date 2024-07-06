using Iwan.Server.Constants;
using Iwan.Server.Queries.Pages;
using Iwan.Server.Services.Accounts;
using Iwan.Shared;
using Iwan.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Controllers
{
    public class HomeController : BaseController
    {
        protected readonly IStringLocalizer<Localization> _localizer;
        private readonly IMediator _mediator;

        public HomeController(IStringLocalizer<Localization> localizer, IMediator mediator)
        {
            _localizer = localizer;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(string section = "", CancellationToken cancellationToken = default)
        {
            var homePageContent = await _mediator.Send(new GetHomePageContent.Request(), cancellationToken);

            ViewData["SectionToNavigateTo"] = section;
            return View(homePageContent);
        }

        public ActionResult RedirectToDefaultLanguage()
        {
            var lang = CurrentLanguage;
            if (!AppLanguages.All().Contains(lang))
            {
                lang = AppLanguages.English.Culture;
            }

            return RedirectToAction("Index", new { lang });
        }

        private string CurrentLanguage
        {
            get
            {
                return HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.TwoLetterISOLanguageName.ToLower();
            }
        }
    }
}
