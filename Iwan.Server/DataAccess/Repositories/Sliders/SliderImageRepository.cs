using Iwan.Server.Domain.Sliders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Sliders
{
    public class SliderImageRepository : Repository<SliderImage>, ISliderImageRepository
    {
        public SliderImageRepository(ApplicationDbContext context) : base(context) { }

        public async Task<SliderImage> GetWithImagesAsync(string sliderImageId, CancellationToken cancellationToken = default)
        {
            return await Table.Include(s => s.OriginalImage).Include(s => s.MediumImage)
                .Include(s => s.MobileImage).SingleOrDefaultAsync(s => s.Id == sliderImageId, cancellationToken);
        }

        public async Task<IEnumerable<SliderImage>> GetWithImagesAsync(CancellationToken cancellationToken = default)
        {
            return await Table.Include(s => s.OriginalImage).Include(s => s.MediumImage)
                 .Include(s => s.MobileImage).ToListAsync(cancellationToken);
        }
    }
}
