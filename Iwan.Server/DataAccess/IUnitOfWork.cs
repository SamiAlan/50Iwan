using Iwan.Server.DataAccess.Repositories.Catalog;
using Iwan.Server.DataAccess.Repositories.Common;
using Iwan.Server.DataAccess.Repositories.Compositions;
using Iwan.Server.DataAccess.Repositories.Pages;
using Iwan.Server.DataAccess.Repositories.Media;
using Iwan.Server.DataAccess.Repositories.Products;
using Iwan.Server.DataAccess.Repositories.Sales;
using Iwan.Server.DataAccess.Repositories.Security;
using Iwan.Server.DataAccess.Repositories.Settings;
using Iwan.Server.DataAccess.Repositories.Sliders;
using Iwan.Server.DataAccess.Repositories.Vendors;
using Iwan.Server.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Iwan.Server.DataAccess.Repositories.Jobs;

namespace Iwan.Server.DataAccess
{
    /// <summary>
    /// Represents the applications database context contract for doing database operations
    /// </summary>
    public interface IUnitOfWork
    {
        #region Repos

        DbSet<AppUser> Users { get; }
        IAddressRepository AddressesRepository { get; }
        IBillRepository BillsRepository { get; }
        IRefreshTokenRepository RefreshTokensRepository { get; }
        IBillItemRepository BillItemsRepository { get; }
        ICategoryRepository CategoriesRepository { get; }
        ICategoryImageRepository CategoryImagesRepository { get; }
        ICompositionImageRepository CompositionImagesRepository { get; }
        ICompositionRepository CompositionsRepository { get; }
        IImageRepository ImagesRepository { get; }
        IProductsImagesSettingsRepository ProductsImagesSettingsRepository { get; }
        IProductStateRepository ProductStatesRepository { get; }
        ICategoriesImagesSettingsRepository CategoriesImagesSettingsRepository { get; }
        ICompositionsImagesSettingsRepository CompositionsImagesSettingsRepository { get; }
        ISlidersImagesSettingsRepository SlidersImagesSettingsRepository { get; }
        IAboutUsSectionImagesSettingsRepository AboutUsSectionImagesSettingsRepository { get; }
        IProductCategoryRepository ProductCategoriesRepository { get; }
        IProductImageRepository ProductImagesRepository { get; }
        IProductMainImageRepository ProductMainImagesRepository { get; }
        IProductRepository ProductsRepository { get; }
        ISliderImageRepository SliderImagesRepository { get; }
        ITempImageRepository TempImagesRepository { get; }
        ITempImagesSettingsRepository TempImagesSettingsRepository { get; }
        IVendorRepository VendorsRepository { get; }
        IHeaderSectionRepository HeaderSectionsRepository { get; }
        IServicesSectionRepository ServicesSectionsRepository { get; }
        IContactUsSectionRepository ContactUsSectionsRepository { get; }
        IAboutUsSectionRepository AboutUsSectionsRepository { get; }
        IAboutUsSectionImageRepository AboutUsSectionImagesRepository { get; }
        IInteriorDesignSectionRepository InteriorDesignSectionsRepository { get; }
        IInteriorDesignSectionImageRepository InteriorDesignSectionImagesRepository { get; }
        IColorPickingSectionRepository ColorPickingSectionsRepository { get; }
        IColorRepository ColorsRepository { get; }
        IWatermarkSettingsRepository WatermarkSettingsRepository { get; }
        IJobDetailRepository JobDetailsRepository { get; }

        #endregion

        Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        IDbContextTransaction BeginTransaction();
        IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel);
        int SaveChanges(string userId = null);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(string userId = null, CancellationToken cancellationToken = default);
    }
}
