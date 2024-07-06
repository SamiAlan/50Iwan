using Iwan.Server.Services.Settings;
using Iwan.Shared.Dtos.Settings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Settings.Admin
{
    public class SaveWatermarkSettings
    {
        public record Request(EditWatermarkSettingsDto SettingsDto) : IRequest<WatermarkSettingsDto>;

        public class Handler : IRequestHandler<Request, WatermarkSettingsDto>
        {
            protected readonly ISettingService _settingService;
            protected readonly IQuerySettingService _querySettingService;

            public Handler(ISettingService settingService, IQuerySettingService querySettingService)
            {
                _settingService = settingService;
                _querySettingService = querySettingService;
            }

            public async Task<WatermarkSettingsDto> Handle(Request request, CancellationToken cancellationToken)
            {
                await _settingService.UpdateSettingsAsync(request.SettingsDto, cancellationToken);
                return await _querySettingService.GetWatermarkImageSettingsDetailsAsync(cancellationToken);
            }
        }
    }
}
