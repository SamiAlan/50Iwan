using Iwan.Server.Domain.Jobs;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Jobs
{
    public interface IJobDetailRepository : IRepository<JobDetail>
    {
        Task<JobDetail> GetByJobAsync(string jobId, CancellationToken cancellationToken = default);
    }
}
