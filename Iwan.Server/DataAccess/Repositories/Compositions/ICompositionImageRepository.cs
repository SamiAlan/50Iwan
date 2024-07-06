using Iwan.Server.Domain.Compositions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Compositions
{
    public interface ICompositionImageRepository : IRepository<CompositionImage>
    {
        Task<CompositionImage> GetByCompositionIncludingImagesAsync(string compositionImage, CancellationToken cancellationToken = default);
        Task<IDictionary<string, CompositionImage>> GetCompositionsImagesGroupedByIdAsync(IEnumerable<string> compositionsIds, CancellationToken cancellationToken = default);
        Task<CompositionImage> GetByCompositionIdAsync(string compositionId, CancellationToken cancellationToken);
    }
}
