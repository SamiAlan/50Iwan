using Iwan.Server.Services.Jobs;
using Iwan.Shared.Dtos.Jobs;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Jobs.Admin
{
    public class GetJobsDetails
    {
        public record Request : IRequest<IEnumerable<JobDetailsDto>>;

        public class Handler : IRequestHandler<Request, IEnumerable<JobDetailsDto>>
        {
            protected readonly IQueryJobDetailsService _queryJobDetailsService;

            public Handler(IQueryJobDetailsService queryJobDetailsService)
            {
                _queryJobDetailsService = queryJobDetailsService;
            }

            public async Task<IEnumerable<JobDetailsDto>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryJobDetailsService.GetCurrentJobsDetailsAsync(cancellationToken);
            }
        }
    }
}
