using Iwan.Server.DataAccess;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Infrastructure.DataAccess
{
    [Injected(ServiceLifetime.Scoped, typeof(ITransactionProvider))]
    public class TransactionProvider : ITransactionProvider
    {
        protected readonly IUnitOfWork _applicationDbContext;

        public TransactionProvider(IUnitOfWork applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _applicationDbContext.BeginTransaction();
        }

        public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return _applicationDbContext.BeginTransaction(isolationLevel);
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
            return _applicationDbContext.BeginTransactionAsync(isolationLevel, cancellationToken);
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return _applicationDbContext.BeginTransactionAsync(cancellationToken);
        }
    }
}
