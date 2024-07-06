using Iwan.Server.Services.Settings;
using Iwan.Shared.Dtos.Settings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Settings.Admin
{
    public class SaveCategoriesImagesSettings
    {
        public record Request(CategoriesImagesSettingsDto SettingsDto) : IRequest<CategoriesImagesSettingsDto>;

        public class Handler : IRequestHandler<Request, CategoriesImagesSettingsDto>
        {
            protected readonly ISettingService _settingService;
            protected readonly IQuerySettingService _querySettingService;

            public Handler(ISettingService settingService, IQuerySettingService querySettingService)
            {
                _settingService = settingService;
                _querySettingService = querySettingService;
            }

            public async Task<CategoriesImagesSettingsDto> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _settingService.UpdateSettingsAsync(request.SettingsDto, cancellationToken);
            }
        }
    }
}
