using Iwan.Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IDictionary<string, T>> GetGroupedByIdAsync(IEnumerable<string> entitiesIds, CancellationToken cancellationToken = default);
        Task<T> FindAsync(string entityId, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAsync(IEnumerable<string> entitiesIds, CancellationToken cancellationToken = default);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken = default);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        T Edit(T editedEntity);
        IEnumerable<T> EditRange(IEnumerable<T> entities);
        T Attach(T editedEntity);
        IEnumerable<T> AttachRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<bool> ExistAsync(string entityId, CancellationToken cancellationToken = default);
        Task<bool> ExistAsync(IList<string> entityId, CancellationToken cancellationToken = default);
        IQueryable<T> Table { get; }
    }
}
