using Iwan.Server.Domain.Products;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Products
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Product> GetProductByNumberAsync(int number, CancellationToken cancellationToken = default)
        {
            return await Table.SingleOrDefaultAsync(p => p.Number == number, cancellationToken);
        }

        public async Task<bool> HasMainImageAsync(string id, CancellationToken cancellationToken)
        {
            return await _context.ProductMainImagesRepository.AnyAsync(p => p.ProductId == id, cancellationToken);
        }

        public async Task<bool> ProductNumberExistsAsync(int number, CancellationToken cancellationToken = default)
        {
            return await Table.AnyAsync(p => p.Number == number, cancellationToken);
        }
    }
}
