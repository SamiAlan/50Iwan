using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Compositions;
using Iwan.Shared.Options.Compositions;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Compositions
{
    public interface IQueryCompositionService
    {
        Task<CompositionDto> GetCompositionDetailsAsync(string compositionId, bool includeImages = true, CancellationToken cancellationToken = default);
        Task<PagedDto<CompositionDto>> GetCompositionsAsync(GetCompositionsOptions options, CancellationToken cancellationToken = default);
    }
}
