using Iwan.Server.Services.Settings;
using Iwan.Shared.Dtos.Settings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Settings.Admin
{
    public class SaveCompositionsImagesSettings
    {
        public record Request(CompositionsImagesSettingsDto Settings) : IRequest<CompositionsImagesSettingsDto>;

        public class Handler : IRequestHandler<Request, CompositionsImagesSettingsDto>
        {
            protected readonly ISettingService _settingService;
            protected readonly IQuerySettingService _querySettingService;

            public Handler(ISettingService settingService, IQuerySettingService querySettingService)
            {
                _settingService = settingService;
                _querySettingService = querySettingService;
            }

            public async Task<CompositionsImagesSettingsDto> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _settingService.UpdateSettingsAsync(request.Settings, cancellationToken);
            }
        }
    }
}
