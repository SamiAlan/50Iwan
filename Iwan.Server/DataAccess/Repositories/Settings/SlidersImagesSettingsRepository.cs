using Iwan.Server.Domain.Settings;

namespace Iwan.Server.DataAccess.Repositories.Settings
{
    public class SlidersImagesSettingsRepository : Repository<SlidersImagesSettings>, ISlidersImagesSettingsRepository
    {
        public SlidersImagesSettingsRepository(ApplicationDbContext context) : base(context) { }
    }
}
