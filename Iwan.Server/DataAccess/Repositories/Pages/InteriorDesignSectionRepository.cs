using Iwan.Server.Domain.Pages;

namespace Iwan.Server.DataAccess.Repositories.Pages
{
    public class InteriorDesignSectionRepository : Repository<InteriorDesignSection>, IInteriorDesignSectionRepository
    {
        public InteriorDesignSectionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
