using Iwan.Server.DataAccess;
using Iwan.Shared.Dtos.Jobs;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Jobs
{
    [Injected(ServiceLifetime.Scoped, typeof(IQueryJobDetailsService))]
    public class QueryJobDetailsService : IQueryJobDetailsService
    {
        protected readonly IUnitOfWork _context;

        public QueryJobDetailsService(IUnitOfWork context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobDetailsDto>> GetCurrentJobsDetailsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.JobDetailsRepository.Table.Select(j => new JobDetailsDto
            {
                Id = j.Id,
                JobStatusId = j.JobStatusId,
                JobTypeId = j.JobTypeId,
            }).ToListAsync(cancellationToken);
        }
    }
}
