using Iwan.Server.Domain.Media;

namespace Iwan.Server.DataAccess.Repositories.Media
{
    public class TempImageRepository : Repository<TempImage>, ITempImageRepository
    {
        public TempImageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
