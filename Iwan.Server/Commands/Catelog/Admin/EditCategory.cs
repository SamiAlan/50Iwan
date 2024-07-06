using Iwan.Server.Services.Catalog;
using Iwan.Shared.Dtos.Catalog;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Catelog.Admin
{
    public class EditCategory
    {
        public record Request(EditCategoryDto Category) : IRequest<CategoryDto>;

        public class Handler : IRequestHandler<Request, CategoryDto>
        {
            protected readonly ICategoryService _catgoryService;
            protected readonly IQueryCategoryService _queryCatgoryService;

            public Handler(ICategoryService catgoryService, IQueryCategoryService queryCatgoryService)
            {
                _catgoryService = catgoryService;
                _queryCatgoryService = queryCatgoryService;
            }

            public async Task<CategoryDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var category = await _catgoryService.EditCategoryAsync(request.Category, cancellationToken);

                return await _queryCatgoryService.GetCategoryDetailsAsync(category.Id, false, cancellationToken);
            }
        }
    }
}