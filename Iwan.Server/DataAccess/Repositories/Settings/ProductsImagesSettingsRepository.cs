using Iwan.Server.Domain.Settings;

namespace Iwan.Server.DataAccess.Repositories.Settings
{
    public class ProductsImagesSettingsRepository : Repository<ProductsImagesSettings>, IProductsImagesSettingsRepository
    {
        public ProductsImagesSettingsRepository(ApplicationDbContext context) : base(context) { }
    }
}
