using Iwan.Server.Constants;
using Iwan.Server.Infrastructure.Media;
using Iwan.Server.Services.Accounts;
using Iwan.Server.Services.Compositions;
using Iwan.Shared.Dtos.Compositions;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Iwan.Server.Commands.Compositions.Admin
{
    public class ChangeCompositionImage
    {
        public record Request(ChangeCompositionImageDto CompositionImage) : IRequest<CompositionImageDto>;

        public class Handler : IRequestHandler<Request, CompositionImageDto>
        {
            protected readonly ICompositionService _compositionService;
            protected readonly IAppImageHelper _appUrlHelper;
            protected readonly ILoggedInUserProvider _loggedInUserProvider;

            public Handler(ICompositionService compositionService, IAppImageHelper appUrlHelper,
                ILoggedInUserProvider loggedInUserProvider)
            {
                _compositionService = compositionService;
                _appUrlHelper = appUrlHelper;
                _loggedInUserProvider = loggedInUserProvider;
            }

            public async Task<CompositionImageDto> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        var compositionImage = await _compositionService.ChangeCompositionImageAsync(request.CompositionImage.CompositionId,
                            request.CompositionImage, cancellationToken);

                        scope.Complete();

                        return new CompositionImageDto
                        {
                            OriginalImage = _appUrlHelper.GenerateImageDto(compositionImage.OriginalImage),
                            MediumImage = _appUrlHelper.GenerateImageDto(compositionImage.MediumImage),
                            MobileImage = _appUrlHelper.GenerateImageDto(compositionImage.MobileImage),
                        };
                    }
                    catch (BaseException)  { throw; }
                    catch { throw new ServerErrorException(Responses.Compositions.ErrorAddingComposition); }
                }
            }
        }
    }
}
