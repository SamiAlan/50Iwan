using Iwan.Server.Domain.Jobs;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Jobs
{
    public class JobDetailRepository : Repository<JobDetail>, IJobDetailRepository
    {
        public JobDetailRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<JobDetail> GetByJobAsync(string jobId, CancellationToken cancellationToken = default)
        {
            return await Table.Where(d => d.JobId == jobId).SingleOrDefaultAsync(cancellationToken);
        }
    }
}
