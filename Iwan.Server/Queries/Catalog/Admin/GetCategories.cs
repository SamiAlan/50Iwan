using Iwan.Server.Services.Catalog;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Options.Catalog;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Catelog.Admin
{
    public class GetCategories
    {
        public record Request(GetCategoriesOptions Options) : IRequest<PagedDto<CategoryDto>>;

        public class Handler : IRequestHandler<Request, PagedDto<CategoryDto>>
        {
            protected readonly IQueryCategoryService _queryCategoryService;

            public Handler(IQueryCategoryService queryCategoryService)
            {
                _queryCategoryService = queryCategoryService;
            }

            public async Task<PagedDto<CategoryDto>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryCategoryService.GetCategoriesAsync(request.Options, cancellationToken);
            }
        }
    }
}
