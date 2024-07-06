using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Shared.Common;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Dashboard;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardDataAsync(CancellationToken cancellationToken = default);
    }

    [Injected(ServiceLifetime.Scoped, typeof(IDashboardService))]
    public class DashboardService : IDashboardService
    {
        protected readonly HttpClient _client;

        public DashboardService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient(HttpClientsNames.Base);
        }

        public async Task<DashboardDto> GetDashboardDataAsync(CancellationToken cancellationToken = default)
        {
            return await _client.GetAndReturnOrThrowAsync<DashboardDto, ApiErrorResponse>
                (Routes.Api.Admin.Dashboard.BASE, cancellationToken);
        }
    }
}
