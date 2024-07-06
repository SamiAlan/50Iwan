using Iwan.Server.Constants;
using Iwan.Server.Options;
using Iwan.Server.Queries.Pages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Controllers
{

    public class ProductController : BaseController
    {
        protected readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get products in category (paged)
        public async Task<IActionResult> Index([FromQuery]GetProductsPageOptions options, CancellationToken cancellationToken = default)
        {
            if (ServerState.WorkingOnProducts) return Content("Server is updating. Please revisit the website in a bit:)");

            options.PageSize = 12;

            var pageContent = await _mediator.Send(new GetProductsPageContent.Request(options), cancellationToken);

            return View(pageContent);
        }

        // Get product details
        public async Task<IActionResult> Details(string id, string categoryId, CancellationToken cancellationToken = default)
        {
            if (ServerState.WorkingOnProducts) return Content("Server is updating. Please revisit the website in a bit:)");
            var pageContent = await _mediator.Send(new GetProductDetailsPageContent.Request(id, categoryId), cancellationToken);

            if (pageContent.Product == null)
                return NotFound();

            return View(pageContent);
        }

        // Search for products (paged)
        public async Task<IActionResult> Search([FromQuery]GetSearchProductsOptions options, CancellationToken cancellationToken = default)
        {
            if (ServerState.WorkingOnProducts) return Content("Server is updating. Please revisit the website in a bit:)");
            options.PageSize = 12;
            var pageContent = await _mediator.Send(new GetSearchPageContent.Request(options), cancellationToken);

            return View(pageContent);
        }
    }
}
