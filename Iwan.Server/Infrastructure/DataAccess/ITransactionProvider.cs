using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Infrastructure.DataAccess
{
    public interface ITransactionProvider
    {
        Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        IDbContextTransaction BeginTransaction();
        IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel);
    }
}
