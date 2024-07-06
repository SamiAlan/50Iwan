using Iwan.Server.Domain.Settings;

namespace Iwan.Server.DataAccess.Repositories.Settings
{
    public class AboutUsSectionImagesSettingsRepository : Repository<AboutUsSectionImagesSettings>, IAboutUsSectionImagesSettingsRepository
    {
        public AboutUsSectionImagesSettingsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

