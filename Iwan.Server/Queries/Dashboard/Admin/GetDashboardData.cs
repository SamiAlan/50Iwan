using Iwan.Server.Services.Dashoard;
using Iwan.Shared.Dtos.Dashboard;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Dashboard.Admin
{
    public class GetDashboardData
    {
        public record Request : IRequest<DashboardDto>;

        public class Handler : IRequestHandler<Request, DashboardDto>
        {
            protected readonly IDashboardQueryService _dashboardQueryService;

            public Handler(IDashboardQueryService dashboardQueryService)
            {
                _dashboardQueryService = dashboardQueryService;
            }

            public async Task<DashboardDto> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _dashboardQueryService.GetDashboardDataAsync(cancellationToken);
            }
        }
    }
}
