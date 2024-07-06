using Iwan.Server.Domain.Pages;

namespace Iwan.Server.DataAccess.Repositories.Pages
{
    public class ContactUsSectionRepository : Repository<ContactUsSection>, IContactUsSectionRepository
    {
        public ContactUsSectionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
