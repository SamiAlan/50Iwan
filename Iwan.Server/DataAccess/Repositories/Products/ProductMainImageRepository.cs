using Iwan.Server.Domain.Products;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Products
{
    public class ProductMainImageRepository : Repository<ProductMainImage>, IProductMainImageRepository
    {
        public ProductMainImageRepository(ApplicationDbContext context) : base(context) { }

        public async Task<ProductMainImage> GetByProductIdAndIncludeImagesAsync(string productId, CancellationToken cancellationToken)
        {
            return await Table.Include(i => i.OriginalImage)
                .Include(i => i.MediumImage)
                .Include(i => i.SmallImage)
                .Include(i => i.MobileImage)
                .SingleOrDefaultAsync(i => i.ProductId == productId, cancellationToken);
        }

        public async Task<ProductMainImage> GetAndIncludeImagesAsync(string productMainImageId, CancellationToken cancellationToken)
        {
            return await Table.Include(i => i.OriginalImage)
                .Include(i => i.MediumImage)
                .Include(i => i.SmallImage)
                .Include(i => i.MobileImage)
                .SingleOrDefaultAsync(i => i.Id == productMainImageId, cancellationToken);
        }

        public async Task<IDictionary<string, ProductMainImage>> GetProductsImagesGroupedByIdAsync(IEnumerable<string> productsIds, CancellationToken cancellationToken = default)
        {
            return await Table.Include(i => i.OriginalImage).Include(i => i.MediumImage)
                .Include(i => i.SmallImage).Include(i => i.MobileImage)
                .Where(i => productsIds.Contains(i.ProductId))
                .ToDictionaryAsync(i => i.ProductId, i => i, cancellationToken);
        }

        public async Task<ProductMainImage> GetProductMainImageAsync(string productId, CancellationToken token = default)
        {
            return await Table.SingleOrDefaultAsync(i => i.ProductId == productId, token);
        }
    }
}
