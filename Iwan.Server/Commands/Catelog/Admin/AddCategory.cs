using Iwan.Server.Constants;
using Iwan.Server.Services.Catalog;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Iwan.Server.Commands.Catelog.Admin
{
    public class AddCategory
    {
        public record Request(AddCategoryDto Category) : IRequest<CategoryDto>;

        public class Handler : IRequestHandler<Request, CategoryDto>
        {
            protected readonly ICategoryService _categoryService;
            protected readonly IQueryCategoryService _queryCategoryService;

            public Handler(ICategoryService categoryService, IQueryCategoryService queryCategoryService)

            {
                _categoryService = categoryService;
                _queryCategoryService = queryCategoryService;
            }

            public async Task<CategoryDto> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        var category = await _categoryService.AddCategoryAsync(request.Category, cancellationToken);

                        var categoryDto = await _queryCategoryService.GetCategoryDetailsAsync(category.Id, true, cancellationToken);
                        
                        scope.Complete();

                        return categoryDto;
                    }
                    catch (BaseException) { throw; }
                    catch
                    {
                        throw new ServerErrorException(Responses.Categories.ServerErrorWhenAddingCategory);
                    }
                }
            }
        }
    }
}
