using Iwan.Server.Domain.Compositions;
using Iwan.Shared.Dtos.Compositions;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Compositions
{
    public interface ICompositionService
    {
        Task<Composition> AddCompositionAsync(AddCompositionDto compositionToAdd, CancellationToken cancellationToken);
        Task DeleteCompositionAsync(string compositionId, CancellationToken cancellationToken);
        Task<Composition> EditCompositionAsync(EditCompositionDto editedComposition, CancellationToken cancellationToken);
        Task<CompositionImage> ChangeCompositionImageAsync(string compositionId, ChangeCompositionImageDto compositionImage, CancellationToken cancellationToken);
    }
}
