using Iwan.Server.Domain.Pages;

namespace Iwan.Server.DataAccess.Repositories.Pages
{
    public class ServicesSectionRepository : Repository<ServicesSection>, IServicesSectionRepository
    {
        public ServicesSectionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
