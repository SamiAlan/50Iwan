using Iwan.Server.Domain.Pages;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Pages
{
    public class ColorPickingSectionRepository : Repository<ColorPickingSection>, IColorPickingSectionRepository
    {
        public ColorPickingSectionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ColorPickingSection> GetSectionWithColorsAsync(CancellationToken cancellationToken = default)
        {
            return await Table.Include(s => s.Colors).SingleOrDefaultAsync(cancellationToken);
        }
    }
}
