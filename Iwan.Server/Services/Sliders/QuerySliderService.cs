using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Extensions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Server.Infrastructure.Media;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Sliders;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Options.SliderImages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Sliders
{
    [Injected(ServiceLifetime.Scoped, typeof(IQuerySliderService))]
    public class QuerySliderService : IQuerySliderService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IAppImageHelper _appUrlHelper;

        public QuerySliderService(IUnitOfWork unitOfWork, IAppImageHelper appUrlHelper)
        {
            _unitOfWork = unitOfWork;
            _appUrlHelper = appUrlHelper;
        }

        public async Task<SliderImageDto> GetSliderImageDetailsAsync(string sliderImageId, CancellationToken cancellationToken = default)
        {
            var sliderImage = await _unitOfWork.SliderImagesRepository.GetWithImagesAsync(sliderImageId, cancellationToken);

            if (sliderImage == null)
                throw new NotFoundException(Responses.Sliders.SliderImageNotFound);

            return new SliderImageDto
            {
                Id = sliderImage.Id,
                Order = sliderImage.Order,
                MediumImage = _appUrlHelper.GenerateImageDto(sliderImage.MediumImage),
                MobileImage = _appUrlHelper.GenerateImageDto(sliderImage.MobileImage)
            };
        }

        public async Task<PagedDto<SliderImageDto>> GetSliderImagesDetailsAsync(GetSliderImagesOptions options, CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.SliderImagesRepository.Table.Include(s => s.OriginalImage)
                .Include(s => s.MobileImage).OrderBy(s => s.Order).AsQueryable();

            var totalCount = await query.CountAsync(cancellationToken);

            var pagedCategories = new PagedDto<SliderImageDto>
            {
                PageNumber = options.CurrentPage,
                PageSize = options.PageSize,
                TotalCount = totalCount,
                HasPrevious = options.CurrentPage != 1,
                HasNext = totalCount > (options.CurrentPage * options.PageSize)
            };

            query = query.Skip(options.CurrentPage * options.PageSize).Take(options.PageSize);

            var sliderImages = await query.ToListAsync(cancellationToken);

            return sliderImages.AsPaged(options.CurrentPage, options.PageSize, totalCount, s => new SliderImageDto
            {
                Id = s.Id,
                Order = s.Order,
                MediumImage = _appUrlHelper.GenerateImageDto(s.MediumImage),
                MobileImage = _appUrlHelper.GenerateImageDto(s.MobileImage)
            });
        }
    }
}
