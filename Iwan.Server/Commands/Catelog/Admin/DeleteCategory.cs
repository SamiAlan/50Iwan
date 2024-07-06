using Iwan.Server.Constants;
using Iwan.Server.Services.Catalog;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Iwan.Server.Commands.Catelog.Admin
{
    public class DeleteCategory
    {
        public record Request(string CategoryId) : IRequest;

        public class Handler : IRequestHandler<Request, Unit>
        {
            protected readonly ICategoryService _categoryService;

            public Handler(ICategoryService categoryService)
            {
                _categoryService = categoryService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        await _categoryService.DeleteCategoryAsync(request.CategoryId, cancellationToken);

                        scope.Complete();

                        return Unit.Value;
                    }
                    catch (BaseException) { throw; }
                    catch { throw new ServerErrorException(Responses.Categories.ErrorDeletingCategory); }
                }
            }
        }
    }
}
