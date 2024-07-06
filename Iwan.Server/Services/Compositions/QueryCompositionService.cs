using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Server.Infrastructure.Media;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Compositions;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Options.Compositions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Compositions
{
    [Injected(ServiceLifetime.Scoped, typeof(IQueryCompositionService))]
    public class QueryCompositionService : IQueryCompositionService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IAppImageHelper _appUrlHelper;

        public QueryCompositionService(IUnitOfWork unitOfWork, IAppImageHelper appUrlHelper)
        {
            _unitOfWork = unitOfWork;
            _appUrlHelper = appUrlHelper;
        }

        public async Task<CompositionDto> GetCompositionDetailsAsync(string compositionId, bool includeImages = true, CancellationToken
            cancellationToken = default)
        {
            var composition = await _unitOfWork.CompositionsRepository.FindAsync(compositionId, cancellationToken);

            if (composition == null)
                throw new NotFoundException(Responses.Compositions.CompositionNotExist);

            var compositionDto = new CompositionDto
            {
                Id = composition.Id,
                ArabicName = composition.ArabicName,
                EnglishName = composition.EnglishName,
                ArabicDescription = composition.ArabicDescription,
                EnglishDescription = composition.EnglishDescription,
                IsVisible = composition.IsVisible,
                ColorCode = composition.ColorCode,
                ColorTypeId = composition.ColorTypeId
            };

            if (!includeImages)
                return compositionDto;

            var compositionImage = await _unitOfWork.CompositionImagesRepository.GetByCompositionIncludingImagesAsync(composition.Id, cancellationToken);

            compositionDto.Image = new CompositionImageDto
            {
                OriginalImage = _appUrlHelper.GenerateImageDto(compositionImage.OriginalImage),
                MediumImage = _appUrlHelper.GenerateImageDto(compositionImage.MediumImage),
                MobileImage = _appUrlHelper.GenerateImageDto(compositionImage.MobileImage)

            };

            return compositionDto;
        }

        public async Task<PagedDto<CompositionDto>> GetCompositionsAsync(GetCompositionsOptions options, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.CompositionsRepository.Table;

            if (!options.Text.IsNullOrEmptyOrWhiteSpaceSafe())
                query = query
                    .Where(c => c.ArabicName.ToLower().Contains(options.Text) ||
                        c.EnglishName.ToLower().Contains(options.Text));

            if (options.OnlyVisible.HasValue)
                query = query.Where(c => c.IsVisible == options.OnlyVisible);

            var totalCount = await query.CountAsync(cancellationToken);

            query = query.Skip(options.CurrentPage * options.PageSize).Take(options.PageSize);

            var pagedCompositions = new PagedDto<CompositionDto>
            {
                PageNumber = options.CurrentPage,
                PageSize = options.PageSize,
                TotalCount = totalCount,
                HasNext = totalCount > (options.PageSize * options.CurrentPage),
                HasPrevious = options.CurrentPage != 1
            };

            if (!options.IncludeImages)
            {
                pagedCompositions.Data = (await query.ToListAsync(cancellationToken)).Select(c => new CompositionDto
                {
                    Id = c.Id,
                    ArabicName = c.ArabicName,
                    EnglishName = c.EnglishName,
                    ArabicDescription = c.ArabicDescription,
                    EnglishDescription = c.EnglishDescription,
                    IsVisible = c.IsVisible
                }).ToList();

                return pagedCompositions;
            }

            var compositions = await query.ToListAsync(cancellationToken);

            var compositionsIds = compositions.Select(c => c.Id);

            var compositionsImagesByCategoryId = await _unitOfWork.CompositionImagesRepository.GetCompositionsImagesGroupedByIdAsync(compositionsIds, cancellationToken);

            var compositionsDtos = new List<CompositionDto>();

            foreach (var composition in compositions)
            {
                var categoryImage = compositionsImagesByCategoryId[composition.Id];

                compositionsDtos.Add(new CompositionDto
                {
                    Id = composition.Id,
                    ArabicName = composition.ArabicName,
                    EnglishName = composition.EnglishName,
                    ArabicDescription = composition.ArabicDescription,
                    EnglishDescription = composition.EnglishDescription,
                    IsVisible = composition.IsVisible,
                    Image = new CompositionImageDto
                    {
                        Id = categoryImage.Id,
                        OriginalImage = _appUrlHelper.GenerateImageDto(categoryImage.OriginalImage),
                        MediumImage = _appUrlHelper.GenerateImageDto(categoryImage.MediumImage),
                        MobileImage = _appUrlHelper.GenerateImageDto(categoryImage.MobileImage)
                    }
                });
            }

            pagedCompositions.Data = compositionsDtos;

            return pagedCompositions;
        }
    }
}
