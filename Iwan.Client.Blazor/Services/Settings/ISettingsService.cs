using Iwan.Shared.Dtos.Settings;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Settings
{
    public interface ISettingsService
    {
        Task<ImagesSettingsDto> GetImagesSettingsAsync(CancellationToken cancellationToken = default);
        Task<TempImagesSettingsDto> GetTempImagesSettingsAsync(CancellationToken cancellationToken = default);
        Task<WatermarkSettingsDto> GetWatermarkSettingsAsync(CancellationToken cancellationToken = default);
        Task<CategoriesImagesSettingsDto> UpdateSettingsAsync(CategoriesImagesSettingsDto settings, CancellationToken cancellationToken = default);
        Task<CompositionsImagesSettingsDto> UpdateSettingsAsync(CompositionsImagesSettingsDto settings, CancellationToken cancellationToken = default);
        Task<ProductsImagesSettingsDto> UpdateSettingsAsync(ProductsImagesSettingsDto settings, CancellationToken cancellationToken = default);
        Task<SlidersImagesSettingsDto> UpdateSettingsAsync(SlidersImagesSettingsDto settings, CancellationToken cancellationToken = default);
        Task<AboutUsSectionImagesSettingsDto> UpdateSettingsAsync(AboutUsSectionImagesSettingsDto settings, CancellationToken cancellationToken = default);
        Task<TempImagesSettingsDto> UpdateSettingsAsync(TempImagesSettingsDto settings, CancellationToken cancellationToken = default);
        Task<WatermarkSettingsDto> UpdateSettingsAsync(EditWatermarkSettingsDto settings, CancellationToken cancellationToken = default);
        Task<WatermarkImageDto> ChangeWatermarkImageAsync(ChangeWatermarkImageDto newImage, CancellationToken cancellationToken = default);
    }
}
