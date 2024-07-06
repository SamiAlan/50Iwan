using Iwan.Server.Services.Catalog;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Options.Catalog;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Catalog.Admin
{
    public class GetAllCategories
    {
        public record Request(GetAllCategoriesOptions Options) : IRequest<IEnumerable<CategoryDto>>;

        public class Handler : IRequestHandler<Request, IEnumerable<CategoryDto>>
        {
            protected readonly IQueryCategoryService _queryCategoryService;

            public Handler(IQueryCategoryService queryCategoryService)
            {
                _queryCategoryService = queryCategoryService;
            }

            public async Task<IEnumerable<CategoryDto>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryCategoryService.GetAllCategoriesAsync(request.Options, cancellationToken);
            }
        }
    }
}
