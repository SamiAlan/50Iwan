using Iwan.Server.Domain.Pages;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Pages
{
    public interface IAboutUsSectionImageRepository : IRepository<AboutUsSectionImage>
    {
        Task<AboutUsSectionImage> GetIncludingImagesAsync(string aboutUsImageId, CancellationToken cancellationToken = default);
        Task<IEnumerable<AboutUsSectionImage>> GetImagesSectionAsync(CancellationToken cancellationToken = default);
    }
}
