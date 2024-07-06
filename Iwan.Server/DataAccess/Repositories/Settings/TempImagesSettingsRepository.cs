using Iwan.Server.Domain.Settings;

namespace Iwan.Server.DataAccess.Repositories.Settings
{
    public class TempImagesSettingsRepository : Repository<TempImagesSettings>, ITempImagesSettingsRepository
    {
        public TempImagesSettingsRepository(ApplicationDbContext context) : base(context) { }
    }
}
