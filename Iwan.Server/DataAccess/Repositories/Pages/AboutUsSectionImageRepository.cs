using Iwan.Server.Domain.Pages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Pages
{
    public class AboutUsSectionImageRepository : Repository<AboutUsSectionImage>, IAboutUsSectionImageRepository
    {
        public AboutUsSectionImageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AboutUsSectionImage>> GetImagesSectionAsync(CancellationToken cancellationToken = default)
        {
            return await Table.Include(i => i.OriginalImage).Include(i => i.MediumImage).Include(i => i.MobileImage)
                .ToListAsync(cancellationToken);
        }

        public Task<AboutUsSectionImage> GetIncludingImagesAsync(string aboutUsImageId, CancellationToken cancellationToken = default)
        {
            return Table.Include(i => i.OriginalImage).Include(i => i.MediumImage).Include(i => i.MobileImage)
                .SingleOrDefaultAsync(i => i.Id == aboutUsImageId, cancellationToken);
        }
    }
}
