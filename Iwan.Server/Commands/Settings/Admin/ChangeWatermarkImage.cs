using Iwan.Server.Services.Settings;
using Iwan.Shared.Dtos.Settings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Settings.Admin
{
    public class ChangeWatermarkImage
    {
        public record Request(ChangeWatermarkImageDto NewImage) : IRequest<WatermarkImageDto>;

        public class Handler : IRequestHandler<Request, WatermarkImageDto>
        {
            protected readonly ISettingService _settingService;
            protected readonly IQuerySettingService _querySettingService;

            public Handler(ISettingService settingService, IQuerySettingService querySettingService)
            {
                _settingService = settingService;
                _querySettingService = querySettingService;
            }

            public async Task<WatermarkImageDto> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _settingService.ChangeWatermarkImageAsync(request.NewImage, cancellationToken);
            }
        }
    }
}
