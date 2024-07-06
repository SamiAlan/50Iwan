using Iwan.Server.Infrastructure.Media;
using Iwan.Server.Services.Catalog;
using Iwan.Shared.Dtos.Catalog;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Catelog.Admin
{
    public class EditCategoryImage
    {
        public record Request(EditCategoryImageDto CategoryImage) : IRequest<CategoryImageDto>;

        public class Handler : IRequestHandler<Request, CategoryImageDto>
        {
            protected readonly ICategoryService _categoryService;
            protected readonly IAppImageHelper _appUrlHelper;

            public Handler(ICategoryService categoryService, IAppImageHelper appUrlHelper)
            {
                _categoryService = categoryService;
                _appUrlHelper = appUrlHelper;
            }

            public async Task<CategoryImageDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var categoryImage = await _categoryService.EditCategoryImageAsync(request.CategoryImage, cancellationToken);

                //var backgroundlessCroppedImage = categoryImage.BackgroundlessCroppedImage;
                //var backgroundlessMediumImage = categoryImage.BackgroundlessMediumImage;
                //var backgroundlessThumbnailImage = categoryImage.BackgroundlessThumbnailImage;
                //var backgroundlessMobileImage = categoryImage.BackgroundlessMobileImage;

                return new CategoryImageDto
                {
                    //Id = categoryImage.Id,
                    //BackgroundlessCroppedImage = _appUrlHelper.GenerateImageDto(backgroundlessCroppedImage),
                    //BackgroundlessMediumImage = _appUrlHelper.GenerateImageDto(backgroundlessMediumImage),
                    //BackgroundlessThumbnailImage = _appUrlHelper.GenerateImageDto(backgroundlessThumbnailImage),
                    //BackgroundlessMobileImage = _appUrlHelper.GenerateImageDto(backgroundlessMobileImage)
                };
            }
        }
    }
}
