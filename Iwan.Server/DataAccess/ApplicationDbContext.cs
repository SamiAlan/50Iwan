using Iwan.Server.DataAccess.Extensions;
using Iwan.Server.DataAccess.Mappings;
using Iwan.Server.DataAccess.Repositories.Catalog;
using Iwan.Server.DataAccess.Repositories.Common;
using Iwan.Server.DataAccess.Repositories.Compositions;
using Iwan.Server.DataAccess.Repositories.Jobs;
using Iwan.Server.DataAccess.Repositories.Media;
using Iwan.Server.DataAccess.Repositories.Pages;
using Iwan.Server.DataAccess.Repositories.Products;
using Iwan.Server.DataAccess.Repositories.Sales;
using Iwan.Server.DataAccess.Repositories.Security;
using Iwan.Server.DataAccess.Repositories.Settings;
using Iwan.Server.DataAccess.Repositories.Sliders;
using Iwan.Server.DataAccess.Repositories.Vendors;
using Iwan.Server.Domain.Catelog;
using Iwan.Server.Domain.Common;
using Iwan.Server.Domain.Compositions;
using Iwan.Server.Domain.Jobs;
using Iwan.Server.Domain.Media;
using Iwan.Server.Domain.Pages;
using Iwan.Server.Domain.Products;
using Iwan.Server.Domain.Sales;
using Iwan.Server.Domain.Security;
using Iwan.Server.Domain.Settings;
using Iwan.Server.Domain.Sliders;
using Iwan.Server.Domain.Users;
using Iwan.Server.Domain.Vendors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess
{
    /// <summary>
    /// Represents the applications database context class for doing database operations
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<AppUser>, IUnitOfWork
    {
        #region DbSets

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<TempImagesSettings> TempImagesSettings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryImage> CategoryImages { get; set; }
        public DbSet<Composition> Compositions { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<TempImage> TempImages { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillItem> BillItems { get; set; }
        public DbSet<SliderImage> SliderImages { get; set; }
        public DbSet<ProductsImagesSettings> ProductsImagesSettings { get; set; }
        public DbSet<CategoriesImagesSettings> CategoriesImagesSettings { get; set; }
        public DbSet<CompositionsImagesSettings> CompositionsImagesSettings { get; set; }
        public DbSet<SlidersImagesSettings> SlidersImagesSettings { get; set; }
        public DbSet<AboutUsSectionImagesSettings> AboutUsSectionImagesSettings { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<HeaderSection> HeaderSections { get; set; }
        public DbSet<ServicesSection> ServicesSections { get; set; }
        public DbSet<ContactUsSection> ContactUsSections { get; set; }
        public DbSet<AboutUsSection> AboutUsSections { get; set; }
        public DbSet<AboutUsSectionImage> AboutUsSectionImages { get; set; }
        public DbSet<InteriorDesignSection> InteriorDesignSections { get; set; }
        public DbSet<InteriorDesignSectionImage> InteriorDesignSectionImages { get; set; }
        public DbSet<WatermarkSettings> WatermarkSettings { get; set; }
        public DbSet<JobDetail> JobDetails { get; set; }

        #endregion

        #region Repositories

        private ICategoryRepository _categoryRepository;
        public ICategoryRepository CategoriesRepository
        {
            get
            {
                if (_categoryRepository is null) _categoryRepository = new CategoryRepository(this);
                return _categoryRepository;
            }
        }

        private IVendorRepository _vendorRepository;
        public IVendorRepository VendorsRepository
        {
            get
            {
                if (_vendorRepository is null) _vendorRepository = new VendorRepository(this);
                return _vendorRepository;
            }
        }

        private IAddressRepository _addressRepository;
        public IAddressRepository AddressesRepository
        {
            get
            {
                if (_addressRepository is null) _addressRepository = new AddressRepository(this);
                return _addressRepository;
            }
        }

        private IRefreshTokenRepository _refreshTokensRepository;
        public IRefreshTokenRepository RefreshTokensRepository
        {
            get
            {
                if (_refreshTokensRepository is null) _refreshTokensRepository = new RefreshTokenRepository(this);
                return _refreshTokensRepository;
            }
        }

        private IProductRepository _productsRepository;
        public IProductRepository ProductsRepository
        {
            get
            {
                if (_productsRepository is null) _productsRepository = new ProductRepository(this);
                return _productsRepository;
            }
        }

        private IProductStateRepository _productStatesRepository;
        public IProductStateRepository ProductStatesRepository
        {
            get
            {
                if (_productStatesRepository is null) _productStatesRepository = new ProductStateRepository(this);
                return _productStatesRepository;
            }
        }

        private IProductMainImageRepository _productMainImagesRepository;
        public IProductMainImageRepository ProductMainImagesRepository
        {
            get
            {
                if (_productMainImagesRepository is null) _productMainImagesRepository = new ProductMainImageRepository(this);
                return _productMainImagesRepository;
            }
        }

        private ITempImagesSettingsRepository _tempImagesSettingsRepository;
        public ITempImagesSettingsRepository TempImagesSettingsRepository
        {
            get
            {
                if (_tempImagesSettingsRepository is null) _tempImagesSettingsRepository = new TempImagesSettingsRepository(this);
                return _tempImagesSettingsRepository;
            }
        }

        private ICategoryImageRepository _categoryImagesRepository;
        public ICategoryImageRepository CategoryImagesRepository
        {
            get
            {
                if (_categoryImagesRepository is null) _categoryImagesRepository = new CategoryImageRepository(this);
                return _categoryImagesRepository;
            }
        }

        private ICompositionRepository _compositionsRepository;
        public ICompositionRepository CompositionsRepository
        {
            get
            {
                if (_compositionsRepository is null) _compositionsRepository = new CompositionRepository(this);
                return _compositionsRepository;
            }
        }

        private ICompositionImageRepository _compositionImagesRepository;
        public ICompositionImageRepository CompositionImagesRepository
        {
            get
            {
                if (_compositionImagesRepository is null) _compositionImagesRepository = new CompositionImageRepository(this);
                return _compositionImagesRepository;
            }
        }

        private IImageRepository _imagesRepository;
        public IImageRepository ImagesRepository
        {
            get
            {
                if (_imagesRepository is null) _imagesRepository = new ImageRepository(this);
                return _imagesRepository;
            }
        }

        private ITempImageRepository _tempImagesRepository;
        public ITempImageRepository TempImagesRepository
        {
            get
            {
                if (_tempImagesRepository is null) _tempImagesRepository = new TempImageRepository(this);
                return _tempImagesRepository;
            }
        }

        private IProductCategoryRepository _productCategoriesRepository;
        public IProductCategoryRepository ProductCategoriesRepository
        {
            get
            {
                if (_productCategoriesRepository is null) _productCategoriesRepository = new ProductCategoryRepository(this);
                return _productCategoriesRepository;
            }
        }

        private IProductImageRepository _productImagesRepository;
        public IProductImageRepository ProductImagesRepository
        {
            get
            {
                if (_productImagesRepository is null) _productImagesRepository = new ProductImageRepository(this);
                return _productImagesRepository;
            }
        }

        private IBillRepository _billsRepository;
        public IBillRepository BillsRepository
        {
            get
            {
                if (_billsRepository is null) _billsRepository = new BillRepository(this);
                return _billsRepository;
            }
        }

        private IBillItemRepository _billItemsRepository;
        public IBillItemRepository BillItemsRepository
        {
            get
            {
                if (_billItemsRepository is null) _billItemsRepository = new BillItemRepository(this);
                return _billItemsRepository;
            }
        }

        private ISliderImageRepository _sliderImagesRepository;
        public ISliderImageRepository SliderImagesRepository
        {
            get
            {
                if (_sliderImagesRepository is null) _sliderImagesRepository = new SliderImageRepository(this);
                return _sliderImagesRepository;
            }
        }

        private IAboutUsSectionImagesSettingsRepository _aboutUsSectionImagesSettingsRepository;
        public IAboutUsSectionImagesSettingsRepository AboutUsSectionImagesSettingsRepository
        {
            get
            {
                if (_aboutUsSectionImagesSettingsRepository is null) _aboutUsSectionImagesSettingsRepository = new AboutUsSectionImagesSettingsRepository(this);
                return _aboutUsSectionImagesSettingsRepository;
            }
        }

        private IProductsImagesSettingsRepository _productsImagesSettingsRepository;
        public IProductsImagesSettingsRepository ProductsImagesSettingsRepository
        {
            get
            {
                if (_productsImagesSettingsRepository is null) _productsImagesSettingsRepository = new ProductsImagesSettingsRepository(this);
                return _productsImagesSettingsRepository;
            }
        }

        private ICategoriesImagesSettingsRepository _categoriesImagesSettingsRepository;
        public ICategoriesImagesSettingsRepository CategoriesImagesSettingsRepository
        {
            get
            {
                if (_categoriesImagesSettingsRepository is null) _categoriesImagesSettingsRepository = new CategoriesImagesSettingsRepository(this);
                return _categoriesImagesSettingsRepository;
            }
        }

        private ICompositionsImagesSettingsRepository _compositionsImagesSettingsRepository;
        public ICompositionsImagesSettingsRepository CompositionsImagesSettingsRepository
        {
            get
            {
                if (_compositionsImagesSettingsRepository is null) _compositionsImagesSettingsRepository = new CompositionsImagesSettingsRepository(this);
                return _compositionsImagesSettingsRepository;
            }
        }

        private ISlidersImagesSettingsRepository _slidersImagesSettingsRepository;
        public ISlidersImagesSettingsRepository SlidersImagesSettingsRepository
        {
            get
            {
                if (_slidersImagesSettingsRepository is null) _slidersImagesSettingsRepository = new SlidersImagesSettingsRepository(this);
                return _slidersImagesSettingsRepository;
            }
        }

        private IHeaderSectionRepository _headerSectionsRepository;
        public IHeaderSectionRepository HeaderSectionsRepository
        {
            get
            {
                if (_headerSectionsRepository is null) _headerSectionsRepository = new HeaderSectionRepository(this);
                return _headerSectionsRepository;
            }
        }

        private IServicesSectionRepository _servicesSectionsRepository;
        public IServicesSectionRepository ServicesSectionsRepository
        {
            get
            {
                if (_servicesSectionsRepository is null) _servicesSectionsRepository = new ServicesSectionRepository(this);
                return _servicesSectionsRepository;
            }
        }

        private IContactUsSectionRepository _contactUsSectionsRepository;
        public IContactUsSectionRepository ContactUsSectionsRepository
        {
            get
            {
                if (_contactUsSectionsRepository is null) _contactUsSectionsRepository = new ContactUsSectionRepository(this);
                return _contactUsSectionsRepository;
            }
        }

        private IAboutUsSectionRepository _aboutUsSectionsRepository;
        public IAboutUsSectionRepository AboutUsSectionsRepository
        {
            get
            {
                if (_aboutUsSectionsRepository is null) _aboutUsSectionsRepository = new AboutUsSectionRepository(this);
                return _aboutUsSectionsRepository;
            }
        }

        private IAboutUsSectionImageRepository _aboutUsSectionImagesRepository;
        public IAboutUsSectionImageRepository AboutUsSectionImagesRepository
        {
            get
            {
                if (_aboutUsSectionImagesRepository is null) _aboutUsSectionImagesRepository = new AboutUsSectionImageRepository(this);
                return _aboutUsSectionImagesRepository;
            }
        }

        private IInteriorDesignSectionRepository _interiorDesignSectionsRepository;
        public IInteriorDesignSectionRepository InteriorDesignSectionsRepository
        {
            get
            {
                if (_interiorDesignSectionsRepository is null) _interiorDesignSectionsRepository = new InteriorDesignSectionRepository(this);
                return _interiorDesignSectionsRepository;
            }
        }

        private IInteriorDesignSectionImageRepository _interiorDesignSectionImagesRepository;
        public IInteriorDesignSectionImageRepository InteriorDesignSectionImagesRepository
        {
            get
            {
                if (_interiorDesignSectionImagesRepository is null) _interiorDesignSectionImagesRepository = new InteriorDesignSectionImageRepository(this);
                return _interiorDesignSectionImagesRepository;
            }
        }

        private IColorPickingSectionRepository _colorPickingSectionsRepository;
        public IColorPickingSectionRepository ColorPickingSectionsRepository
        {
            get
            {
                if (_colorPickingSectionsRepository is null) _colorPickingSectionsRepository = new ColorPickingSectionRepository(this);
                return _colorPickingSectionsRepository;
            }
        }

        private IColorRepository _colorsRepository;
        public IColorRepository ColorsRepository
        {
            get
            {
                if (_colorsRepository is null) _colorsRepository = new ColorRepository(this);
                return _colorsRepository;
            }
        }

        private IWatermarkSettingsRepository _watermarkSettingsRepository;
        public IWatermarkSettingsRepository WatermarkSettingsRepository
        {
            get
            {
                if (_watermarkSettingsRepository is null) _watermarkSettingsRepository = new WatermarkSettingsRepository(this);
                return _watermarkSettingsRepository;
            }
        }

        private IJobDetailRepository _jobDetailsRepository;
        public IJobDetailRepository JobDetailsRepository
        {
            get
            {
                if (_jobDetailsRepository is null) _jobDetailsRepository = new JobDetailRepository(this);
                return _jobDetailsRepository;
            }
        }

        #endregion

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options) { }

        /// <summary>
        /// Handling when creating the models
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Apply base class handling
            base.OnModelCreating(builder);

            // Apply all builders and configurations
            Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => typeof(IEntityBuilder).IsAssignableFrom(type) &&
                               !type.IsAbstract && !type.IsInterface)
                .ToList().ForEach(type =>
                {
                    ((IEntityBuilder)Activator.CreateInstance(type))
                        .ApplyConfiguration(builder);
                });

            // Seed data
            builder.SeedDefaultUsers();
        }

        /// <summary>
        /// Saves changes to the database synchronously
        /// </summary>
        /// <param name="userId">current user identifier</param>
        public int SaveChanges(string userId = null)
        {
            // Manage added or changed entities
            // ManageEntitiesInChangeTracker(userId);

            // Apply base SaveChanges
            return base.SaveChanges();
        }

        /// <summary>
        /// Saves changes to the database asynchronously
        /// </summary>
        public override Task<int> SaveChangesAsync(CancellationToken token = default)
        {
            // Apply base SaveChangesAsync
            return base.SaveChangesAsync(token);
        }

        /// <summary>
        /// Saves changes to the database asynchronously
        /// </summary>
        /// <param name="userId">current user identifier</param>
        public Task<int> SaveChangesAsync(string userId = null, CancellationToken token = default)
        {
            // Manage added or changed entities
            // ManageEntitiesInChangeTracker(userId);

            // Apply base SaveChangesAsync
            return base.SaveChangesAsync(token);
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
            return Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Database.BeginTransactionAsync(cancellationToken);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return Database.BeginTransaction(isolationLevel);
        }
    }
}
