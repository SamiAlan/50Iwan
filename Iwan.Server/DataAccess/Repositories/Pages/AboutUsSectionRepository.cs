using Iwan.Server.Domain.Pages;

namespace Iwan.Server.DataAccess.Repositories.Pages
{
    public class AboutUsSectionRepository : Repository<AboutUsSection>, IAboutUsSectionRepository
    {
        public AboutUsSectionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
