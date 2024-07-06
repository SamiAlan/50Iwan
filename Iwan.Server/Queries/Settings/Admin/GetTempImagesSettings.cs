using Iwan.Server.Domain.Settings;
using Iwan.Server.Services.Settings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Settings.Admin
{
    public class GetTempImagesSettings
    {
        public record Request : IRequest<TempImagesSettings>;

        public class Handler : IRequestHandler<Request, TempImagesSettings>
        {
            protected readonly IQuerySettingService _querySettingService;

            public Handler(IQuerySettingService querySettingService)
            {
                _querySettingService = querySettingService;
            }

            public async Task<TempImagesSettings> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _querySettingService.GetTempImagesSettingsAsync(cancellationToken);
            }
        }
    }
}
