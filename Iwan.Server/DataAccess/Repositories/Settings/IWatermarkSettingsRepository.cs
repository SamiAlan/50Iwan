using Iwan.Server.Domain.Settings;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Settings
{
    public interface IWatermarkSettingsRepository : IRepository<WatermarkSettings>
    {
        Task<WatermarkSettings> GetWithImageAsync(CancellationToken cancellationToken = default);
    }
}
