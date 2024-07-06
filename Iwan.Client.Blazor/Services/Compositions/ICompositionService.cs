using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Compositions;
using Iwan.Shared.Options.Compositions;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Compositions
{
    public interface ICompositionService
    {
        Task<CompositionDto> AddCompositionAsync(AddCompositionDto composition, CancellationToken cancellationToken = default);
        Task<CompositionDto> EditCompositionAsync(EditCompositionDto composition, CancellationToken cancellationToken = default);
        Task<PagedDto<CompositionDto>> GetCompositionsAsync(GetCompositionsOptions options, CancellationToken cancellationToken = default);
        Task<CompositionDto> GetCompositionByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<CompositionImageDto> ChangeCompositionImageAsync(ChangeCompositionImageDto compositionImage, CancellationToken cancellationToken = default);
        Task DeleteCompositionAsync(string id, CancellationToken cancellationToken = default);
    }
}
