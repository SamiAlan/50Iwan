using Iwan.Server.Models.Catalog;
using Iwan.Server.Services.Catalog;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Catelog
{
    public class GetCategories
    {
        public record Request : IRequest<IEnumerable<CategoryViewModel>>;

        public class Handler : IRequestHandler<Request, IEnumerable<CategoryViewModel>>
        {
            protected readonly IQueryCategoryService _queryCategoryService;

            public Handler(IQueryCategoryService queryCategoryService)
            {
                _queryCategoryService = queryCategoryService;
            }

            public async Task<IEnumerable<CategoryViewModel>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryCategoryService.GetParentCategoriesForPublicAsync(cancellationToken);
            }
        }
    }
}
