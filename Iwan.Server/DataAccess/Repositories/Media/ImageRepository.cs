using Iwan.Server.Domain.Media;

namespace Iwan.Server.DataAccess.Repositories.Media
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
