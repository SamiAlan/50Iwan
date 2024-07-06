using Iwan.Server.Domain.Compositions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Compositions
{
    public class CompositionImageRepository : Repository<CompositionImage>, ICompositionImageRepository
    {
        public CompositionImageRepository(ApplicationDbContext context) : base(context) { }

        public async Task<CompositionImage> GetByCompositionIdAsync(string compositionId, CancellationToken cancellationToken)
        {
            return await Table.SingleOrDefaultAsync(i => i.CompositionId == compositionId, cancellationToken);
        }

        public async Task<CompositionImage> GetByCompositionIncludingImagesAsync(string compositionId, CancellationToken cancellationToken = default)
        {
            return await Table.Include(i => i.OriginalImage)
                .Include(i => i.MediumImage)
                .Include(i => i.MobileImage)
                .SingleOrDefaultAsync(i => i.CompositionId == compositionId, cancellationToken);
        }

        public async Task<IDictionary<string, CompositionImage>> GetCompositionsImagesGroupedByIdAsync(IEnumerable<string> compositionsIds, CancellationToken cancellationToken = default)
        {
            return await Table.Include(i => i.OriginalImage)
                .Include(i => i.MediumImage)
                .Include(i => i.MobileImage)
                .Where(i => compositionsIds.Contains(i.CompositionId))
                .ToDictionaryAsync(i => i.CompositionId, i => i, cancellationToken);
        }
    }
}
