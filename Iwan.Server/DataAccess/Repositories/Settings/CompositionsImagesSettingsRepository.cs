using Iwan.Server.Domain.Settings;

namespace Iwan.Server.DataAccess.Repositories.Settings
{
    public class CompositionsImagesSettingsRepository : Repository<CompositionsImagesSettings>, ICompositionsImagesSettingsRepository
    {
        public CompositionsImagesSettingsRepository(ApplicationDbContext context) : base(context) { }
    }
}
