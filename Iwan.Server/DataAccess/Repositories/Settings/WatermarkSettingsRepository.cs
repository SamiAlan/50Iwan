using Iwan.Server.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Settings
{
    public class WatermarkSettingsRepository : Repository<WatermarkSettings>, IWatermarkSettingsRepository
    {
        public WatermarkSettingsRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<WatermarkSettings> GetWithImageAsync(CancellationToken cancellationToken = default)
        {
            return await Table.Include(s => s.WatermarkImage).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
