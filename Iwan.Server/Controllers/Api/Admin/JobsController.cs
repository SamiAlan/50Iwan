using Iwan.Server.Commands.Jobs;
using Iwan.Server.Events.Jobs;
using Iwan.Server.Queries.Jobs.Admin;
using Iwan.Shared;
using Iwan.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Controllers.Api.Admin
{
    public class JobsController : BaseAdminApiController
    {
        public JobsController(IMediator mediator, IStringLocalizer<Localization> localizer) : base(mediator, localizer)
        {
        }

        // Get all jobs
        [HttpGet]
        [Route(Routes.Api.Admin.Jobs.BASE)]
        public async Task<IActionResult> GetJobsDetails(CancellationToken cancellationToken = default)
        {
            var jobsDetails = await _mediator.Send(new GetJobsDetails.Request(), cancellationToken);

            return Ok(jobsDetails);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Jobs.StartWatermarking)]
        public async Task<IActionResult> StartWatermarking(CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new StartWatermarkingJob.Request(), cancellationToken);

            await _mediator.Publish(new WatermarkingJobStarted(), cancellationToken);

            return Ok();
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Jobs.StartUnWatermarking)]
        public async Task<IActionResult> StartUnWatermarking(CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new StartUnWatermarkingJob.Request(), cancellationToken);

            await _mediator.Publish(new UnWatermarkingJobStarted(), cancellationToken);

            return Ok();
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Jobs.StartProductsResizing)]
        public async Task<IActionResult> StartProductsResizing(CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new StartProductImagesResizingJob.Request(), cancellationToken);

            await _mediator.Publish(new ProductsImagesResizingJobStarted(), cancellationToken);

            return Ok();
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Jobs.StartCategoriesResizing)]
        public async Task<IActionResult> StartCategoriesResizing(CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new StartCategoriesImagesResizingJob.Request(), cancellationToken);

            await _mediator.Publish(new CategoriesImagesResizingJobStarted(), cancellationToken);

            return Ok();
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Jobs.StartCompositionsResizing)]
        public async Task<IActionResult> StartCompositionsResizing(CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new StartCompositionsImagesResizingJob.Request(), cancellationToken);

            await _mediator.Publish(new CompositionsImagesResizingJobStarted(), cancellationToken);

            return Ok();
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Jobs.StartSliderImagesResizing)]
        public async Task<IActionResult> StartSliderImagesResizing(CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new StartSliderImagesResizingJob.Request(), cancellationToken);

            await _mediator.Publish(new SliderImagesResizingJobStarted(), cancellationToken);

            return Ok();
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Jobs.StartAboutUsResizing)]
        public async Task<IActionResult> StartAboutUsResizing(CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new StartAboutUsImagesResizingJob.Request(), cancellationToken);

            await _mediator.Publish(new AboutUsImagesResizingJobStarted(), cancellationToken);

            return Ok();
        }
    }
}
