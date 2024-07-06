using Iwan.Server.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> Table => _context.Set<T>();

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddRangeAsync(entities, cancellationToken);
            return entities;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await Table.AnyAsync(predicate, cancellationToken);
        }

        public T Attach(T editedEntity)
        {
            _context.Set<T>().Attach(editedEntity);
            return editedEntity;
        }

        public IEnumerable<T> AttachRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AttachRange(entities);
            return entities;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await Table.CountAsync(predicate, cancellationToken);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public T Edit(T editedEntity)
        {
            _context.Set<T>().Update(editedEntity);
            return editedEntity;
        }

        public IEnumerable<T> EditRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
            return entities;
        }

        public async Task<bool> ExistAsync(string entityId, CancellationToken cancellationToken = default)
        {
            return await FindAsync(entityId, cancellationToken) != null;
        }

        public async Task<bool> ExistAsync(IList<string> entitiesIds, CancellationToken cancellationToken = default)
        {
            return (await Table.CountAsync(e => entitiesIds.Contains(e.Id), cancellationToken)).Equals(entitiesIds.Count);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await Table.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            return await Table.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await Table.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<T> FindAsync(string entityId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().FindAsync(new object[] { entityId }, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAsync(IEnumerable<string> entitiesIds, CancellationToken cancellationToken = default)
        {
            return await Table.Where(e => entitiesIds.Contains(e.Id)).ToListAsync(cancellationToken);
        }

        public async Task<IDictionary<string, T>> GetGroupedByIdAsync(IEnumerable<string> entitiesIds, CancellationToken cancellationToken = default)
        {
            return await Table.Where(e => entitiesIds.Contains(e.Id)).ToDictionaryAsync(e => e.Id, cancellationToken);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return Table.Where(predicate);
        }
    }
}
