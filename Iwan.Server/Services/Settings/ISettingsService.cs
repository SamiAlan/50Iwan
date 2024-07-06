using Iwan.Server.Domain.Settings;
using Iwan.Shared.Dtos.Settings;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Settings
{
    public interface ISettingService
    {
        Task<CategoriesImagesSettingsDto> UpdateSettingsAsync(CategoriesImagesSettingsDto settings, CancellationToken cancellationToken);
        Task<CompositionsImagesSettingsDto> UpdateSettingsAsync(CompositionsImagesSettingsDto settings, CancellationToken cancellationToken);
        Task<AboutUsSectionImagesSettingsDto> UpdateSettingsAsync(AboutUsSectionImagesSettingsDto settings, CancellationToken cancellationToken);
        Task<ProductsImagesSettingsDto> UpdateSettingsAsync(ProductsImagesSettingsDto settings, CancellationToken cancellationToken);
        Task<SlidersImagesSettingsDto> UpdateSettingsAsync(SlidersImagesSettingsDto settings, CancellationToken cancellationToken);
        Task<TempImagesSettings> UpdateSettingsAsync(TempImagesSettingsDto settings, CancellationToken cancellationToken);
        Task<WatermarkSettings> UpdateSettingsAsync(EditWatermarkSettingsDto newSettings, CancellationToken cancellationToken = default);
        Task<WatermarkImageDto> ChangeWatermarkImageAsync(ChangeWatermarkImageDto newImage, CancellationToken cancellationToken = default);
    }
}
