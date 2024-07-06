using Iwan.Server.Domain.Products;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Products
{
    public interface IProductStateRepository : IRepository<ProductState>
    {
        Task<IEnumerable<ProductState>> GetStatesForProductAsync(string productId, CancellationToken cancellationToken = default);
    }

    public class ProductStateRepository : Repository<ProductState>, IProductStateRepository
    {
        public ProductStateRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<ProductState>> GetStatesForProductAsync(string productId, CancellationToken cancellationToken = default)
        {
            return await Table.Where(s => s.ProductId == productId).ToListAsync(cancellationToken);
        }
    }
}
