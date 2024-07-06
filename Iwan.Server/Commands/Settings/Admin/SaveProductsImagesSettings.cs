using Iwan.Server.Domain.Settings;
using Iwan.Server.Services.Settings;
using Iwan.Shared.Dtos.Settings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Settings.Admin
{
    public class SaveProductsImagesSettings
    {
        public record Request(ProductsImagesSettingsDto SettingsDto) : IRequest<ProductsImagesSettingsDto>;

        public class Handler : IRequestHandler<Request, ProductsImagesSettingsDto>
        {
            protected readonly ISettingService _settingService;
            protected readonly IQuerySettingService _querySettingService;

            public Handler(ISettingService settingService, IQuerySettingService querySettingService)
            {
                _settingService = settingService;
                _querySettingService = querySettingService;
            }

            public async Task<ProductsImagesSettingsDto> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _settingService.UpdateSettingsAsync(request.SettingsDto, cancellationToken);
            }
        }
    }
}
