using Iwan.Server.Domain.Settings;
using Iwan.Server.Services.Settings;
using Iwan.Shared.Dtos.Settings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Settings.Admin
{
    public class GetImagesSettings
    {
        public record Request : IRequest<ImagesSettingsDto>;

        public class Handler : IRequestHandler<Request, ImagesSettingsDto>
        {
            protected readonly IQuerySettingService _querySettingService;

            public Handler(IQuerySettingService querySettingService)
            {
                _querySettingService = querySettingService;
            }

            public async Task<ImagesSettingsDto> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _querySettingService.GetImagesSettingsDetailsAsync(cancellationToken);
            }
        }
    }
}
