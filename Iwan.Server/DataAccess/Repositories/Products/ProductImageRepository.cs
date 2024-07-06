using Iwan.Server.Domain.Products;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Products
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<ProductImage>> GetImagesWithImageModelsAsync(string productId, CancellationToken cancellationToken = default)
        {
            return await _context.ProductImages
                .Include(p => p.OriginalImage)
                .Include(p => p.MediumImage)
                .Include(p => p.SmallImage)
                .Include(p => p.MobileImage)
                .Where(p => p.ProductId == productId)
                .ToListAsync(cancellationToken);
        }

        public async Task<ProductImage> GetImageWithImageModelsAsync(string productImageId, CancellationToken cancellationToken = default)
        {
            return await Table.Include(i => i.OriginalImage)
                .Include(i => i.MediumImage)
                .Include(i => i.SmallImage).Include(i => i.MobileImage)
                .SingleOrDefaultAsync(i => i.Id == productImageId, cancellationToken);
        }
    }
}
