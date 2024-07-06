using Iwan.Server.Services.Catalog;
using Iwan.Shared.Dtos.Catalog;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Catalog.Admin
{
    public class GetCategory
    {
        public record Request(string CategoryId) : IRequest<CategoryDto>;

        public class Handler : IRequestHandler<Request, CategoryDto>
        {
            protected readonly IQueryCategoryService _queryCategoryService;

            public Handler(IQueryCategoryService queryCategoryService)
            {
                _queryCategoryService = queryCategoryService;
            }

            public async Task<CategoryDto> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryCategoryService.GetCategoryDetailsAsync(request.CategoryId, true, cancellationToken);
            }
        }
    }
}
