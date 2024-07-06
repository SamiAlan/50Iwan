using Iwan.Server.Domain.Settings;

namespace Iwan.Server.DataAccess.Repositories.Settings
{
    public class CategoriesImagesSettingsRepository : Repository<CategoriesImagesSettings>, ICategoriesImagesSettingsRepository
    {
        public CategoriesImagesSettingsRepository(ApplicationDbContext context) : base(context) { }
    }
}
