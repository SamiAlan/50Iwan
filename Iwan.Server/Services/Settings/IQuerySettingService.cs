using Iwan.Server.Domain.Settings;
using Iwan.Shared.Dtos.Settings;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Settings
{
    public interface IQuerySettingService
    {
        Task<ProductsImagesSettings> GetProductsImagesSettingsAsync(CancellationToken cancellationToken = default);
        Task<CategoriesImagesSettings> GetCategoriesImagesSettingsAsync(CancellationToken cancellationToken = default);
        Task<CompositionsImagesSettings> GetCompositionsImagesSettingsAsync(CancellationToken cancellationToken = default);
        Task<AboutUsSectionImagesSettings> GetAboutUsSectionImagesSettingsAsync(CancellationToken cancellationToken = default);
        Task<SlidersImagesSettings> GetSlidersImagesSettingsAsync(CancellationToken cancellationToken = default);
        Task<WatermarkSettings> GetWatermarkImageSettingsAsync(CancellationToken cancellationToken = default);
        Task<TempImagesSettings> GetTempImagesSettingsAsync(CancellationToken cancellationToken = default);
        Task<ImagesSettingsDto> GetImagesSettingsDetailsAsync(CancellationToken cancellationToken = default);
        Task<TempImagesSettingsDto> GetTempImagesSettingsDetailsAsync(CancellationToken cancellationToken = default);
        Task<WatermarkSettingsDto> GetWatermarkImageSettingsDetailsAsync(CancellationToken cancellationToken = default);
    }
}
