﻿using Iwan.Server.Constants;
using Iwan.Server.Services.Products;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Iwan.Server.Commands.Products.Admin
{
    public class DeleteProductImage
    {
        public record Request(string ProductImageId) : IRequest<Unit>;

        public class Handler : IRequestHandler<Request, Unit>
        {
            protected readonly IProductService _productService;

            public Handler(IProductService productService)
            {
                _productService = productService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        await _productService.DeleteProductImageAsync(request.ProductImageId, cancellationToken);

                        scope.Complete();

                        return Unit.Value;
                    }
                    catch (BaseException) { throw; }
                    catch { throw new ServerErrorException(Responses.Products.ErrorDeletingProductImage); }
                }
            }
        }
    }
}
