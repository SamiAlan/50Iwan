using Iwan.Server.Domain.Pages;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Pages
{
    public class InteriorDesignSectionImageRepository : Repository<InteriorDesignSectionImage>, IInteriorDesignSectionImageRepository
    {
        public InteriorDesignSectionImageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<InteriorDesignSectionImage> GetInteriorImageAsync(string sectionId, CancellationToken cancellationToken = default)
        {
            return await Table.Include(i => i.MainImage).Include(i => i.MobileImage)
                .SingleOrDefaultAsync(i => i.InteriorDesignSectionId == sectionId, cancellationToken);
        }
    }
}
