using Iwan.Shared.Dtos.Jobs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Jobs
{
    public interface IQueryJobDetailsService
    {
        Task<IEnumerable<JobDetailsDto>> GetCurrentJobsDetailsAsync(CancellationToken cancellationToken = default);
    }
}
