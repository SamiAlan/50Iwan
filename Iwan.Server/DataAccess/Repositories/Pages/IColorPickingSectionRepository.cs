using Iwan.Server.Domain.Pages;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Pages
{
    public interface IColorPickingSectionRepository : IRepository<ColorPickingSection>
    {
        Task<ColorPickingSection> GetSectionWithColorsAsync(CancellationToken cancellationToken = default);
    }
}
