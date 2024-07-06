using Iwan.Server.Domain.Products;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Products
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<bool> ProductNumberExistsAsync(int number, CancellationToken cancellationToken = default);
        Task<Product> GetProductByNumberAsync(int number, CancellationToken cancellationToken = default);
        Task<bool> HasMainImageAsync(string id, CancellationToken cancellationToken);
    }
}
