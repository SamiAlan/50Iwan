using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Shared.Common;
using Iwan.Shared.Constants;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Jobs
{
    [Injected(ServiceLifetime.Scoped, typeof(IBackgroundJobsService))]
    public class BackgroundJobsService : IBackgroundJobsService
    {
        protected readonly HttpClient _client;

        public BackgroundJobsService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient(HttpClientsNames.Base);
        }

        public async Task StartResizingAboutUsImagesAsync(CancellationToken cancellationToken = default)
        {
            await _client.SendOrThrowAsync<ApiErrorResponse>(Routes.Api.Admin.Jobs.StartAboutUsResizing, new object(), HttpMethod.Put, cancellationToken);
        }

        public async Task StartResizingCategoriesImagesAsync(CancellationToken cancellationToken = default)
        {
            await _client.SendOrThrowAsync<ApiErrorResponse>(Routes.Api.Admin.Jobs.StartCategoriesResizing, new object(), HttpMethod.Put, cancellationToken);
        }

        public async Task StartResizingCompositionsImagesAsync(CancellationToken cancellationToken = default)
        {
            await _client.SendOrThrowAsync<ApiErrorResponse>(Routes.Api.Admin.Jobs.StartCompositionsResizing, new object(), HttpMethod.Put, cancellationToken);
        }

        public async Task StartResizingProductsImagesAsync(CancellationToken cancellationToken = default)
        {
            await _client.SendOrThrowAsync<ApiErrorResponse>(Routes.Api.Admin.Jobs.StartProductsResizing, new object(), HttpMethod.Put, cancellationToken);
        }

        public async Task StartResizingSliderImagesAsync(CancellationToken cancellationToken = default)
        {
            await _client.SendOrThrowAsync<ApiErrorResponse>(Routes.Api.Admin.Jobs.StartSliderImagesResizing, new object(), HttpMethod.Put, cancellationToken);
        }

        public async Task StartUnWatermarkingJobAsync(CancellationToken cancellationToken = default)
        {
            await _client.SendOrThrowAsync<ApiErrorResponse>(Routes.Api.Admin.Jobs.StartUnWatermarking, new object(), HttpMethod.Put, cancellationToken);
        }

        public async Task StartWatermarkingJobAsync(CancellationToken cancellationToken = default)
        {
            await _client.SendOrThrowAsync<ApiErrorResponse>(Routes.Api.Admin.Jobs.StartWatermarking, new object(), HttpMethod.Put, cancellationToken);
        }
    }
}
