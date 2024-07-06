using Iwan.Server.Domain.Pages;

namespace Iwan.Server.DataAccess.Repositories.Pages
{
    public class ColorRepository : Repository<Color>, IColorRepository
    {
        public ColorRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
