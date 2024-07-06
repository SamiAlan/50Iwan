using Iwan.Server.Domain.Pages;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Pages
{
    public interface IInteriorDesignSectionImageRepository : IRepository<InteriorDesignSectionImage>
    {
        Task<InteriorDesignSectionImage> GetInteriorImageAsync(string sectionId, CancellationToken cancellationToken = default);
    }
}
