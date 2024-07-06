using Iwan.Server.Constants;
using Iwan.Server.Infrastructure.Media;
using Iwan.Server.Services.Catalog;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Iwan.Server.Commands.Catelog.Admin
{
    public class ChangeCategoryImage
    {
        public record Request(ChangeCategoryImageDto Image) : IRequest<CategoryImageDto>;

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
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        var categoryImage = await _categoryService.ChangeCategoryImageAsync(request.Image.CategoryId,
                            request.Image.Image.Id, cancellationToken);

                        scope.Complete();

                        var backgroundlessCroppedImage = _appUrlHelper.GenerateImageDto(categoryImage.OriginalImage);
                        var backgroundlessMediumImage = _appUrlHelper.GenerateImageDto(categoryImage.MediumImage);
                        var backgroundlessMobileImage = _appUrlHelper.GenerateImageDto(categoryImage.MobileImage);

                        return new CategoryImageDto
                        {
                            Id = categoryImage.Id,
                            OriginalImage = backgroundlessCroppedImage,
                            MediumImage = backgroundlessMediumImage,
                            MobileImage = backgroundlessMobileImage
                        };
                    }
                    catch (BaseException) { throw; }
                    catch { throw new ServerErrorException(Responses.Categories.ErrorEditingCategoryImage); }
                }
            }
        }
    }
}
