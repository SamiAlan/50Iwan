using Iwan.Server.Services.Settings;
using Iwan.Shared.Dtos.Settings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Settings.Admin
{
    public class GetWatermarkSettings
    {
        public record Request() : IRequest<WatermarkSettingsDto>;

        public class Handler : IRequestHandler<Request, WatermarkSettingsDto>
        {
            protected readonly IQuerySettingService _querySettingService;

            public Handler(IQuerySettingService querySettingService)
            {
                _querySettingService = querySettingService;
            }

            public async Task<WatermarkSettingsDto> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _querySettingService.GetWatermarkImageSettingsDetailsAsync(cancellationToken);
            }
        }
    }
}
