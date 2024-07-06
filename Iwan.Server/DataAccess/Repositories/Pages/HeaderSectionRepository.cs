using Iwan.Server.Domain.Pages;

namespace Iwan.Server.DataAccess.Repositories.Pages
{
    public class HeaderSectionRepository : Repository<HeaderSection>, IHeaderSectionRepository
    {
        public HeaderSectionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
