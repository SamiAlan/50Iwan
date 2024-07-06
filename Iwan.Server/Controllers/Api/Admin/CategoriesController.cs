using Iwan.Server.Commands.Catelog.Admin;
using Iwan.Server.Events.Catelog;
using Iwan.Server.Queries.Catalog.Admin;
using Iwan.Server.Queries.Catelog.Admin;
using Iwan.Shared;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Options.Catalog;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Controllers.Api.Admin
{
    public class CategoriesController : BaseAdminApiController
    {
        public CategoriesController(IMediator mediator, IStringLocalizer<Localization> localizer)
            : base(mediator, localizer) { }

        [HttpGet]
        [Route(Routes.Api.Admin.Categories.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCategoriesOptions options, CancellationToken token = default)
        {
            var categories = await _mediator.Send(new GetAllCategories.Request(options), token);

            return Ok(categories);
        }

        [HttpGet]
        [Route(Routes.Api.Admin.Categories.BASE)]
        public async Task<IActionResult> Get([FromQuery] GetCategoriesOptions options, CancellationToken token = default)
        {
            var categories = await _mediator.Send(new GetCategories.Request(options), token);

            return Ok(categories);
        }

        [HttpGet]
        [Route(Routes.Api.Admin.Categories.GetCategory)]
        public async Task<IActionResult> Get(string id, CancellationToken token = default)
        {
            var category = await _mediator.Send(new GetCategory.Request(id), token);

            return Ok(category);
        }

        [HttpPost]
        [Route(Routes.Api.Admin.Categories.BASE)]
        public async Task<IActionResult> Post(AddCategoryDto request, CancellationToken token = default)
        {
            var category = await _mediator.Send(new AddCategory.Request(request), token);

            await _mediator.Publish(new CategoryAddedEvent(category.Id), token);

            return Ok(category);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Categories.BASE)]
        public async Task<IActionResult> Edit(EditCategoryDto request, CancellationToken token = default)
        {
            var category = await _mediator.Send(new EditCategory.Request(request), token);

            await _mediator.Publish(new CategoryEditedEvent(category.Id), token);

            return Ok(category);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Categories.FlipVisibility)]
        public async Task<IActionResult> FlipVisibility(string id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new FlipCategoryVisibility.Request(id), cancellationToken);
            return Ok();
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Categories.ChangeImage)]
        public async Task<IActionResult> ChangeImage(ChangeCategoryImageDto request, CancellationToken token = default)
        {
            var categoryImage = await _mediator.Send(new ChangeCategoryImage.Request(request), token);

            await _mediator.Publish(new CategoryImageChangedEvent(categoryImage.Id), token);

            return Ok(categoryImage);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Categories.EditImage)]
        public async Task<IActionResult> EditImage(EditCategoryImageDto request, CancellationToken token = default)
        {
            var category = await _mediator.Send(new EditCategoryImage.Request(request), token);

            await _mediator.Publish(new CategoryEditedEvent(category.Id), token);

            return Ok(category);
        }

        [HttpDelete]
        [Route(Routes.Api.Admin.Categories.Delete)]
        public async Task<IActionResult> Delete(string id, CancellationToken token = default)
        {
            await _mediator.Send(new DeleteCategory.Request(id), token);

            await _mediator.Publish(new CategoryDeletedEvent(id), token);

            return Ok();
        }
    }
}
