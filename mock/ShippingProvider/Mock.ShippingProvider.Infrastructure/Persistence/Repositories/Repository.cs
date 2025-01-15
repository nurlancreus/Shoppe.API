using Microsoft.EntityFrameworkCore;
using Mock.ShippingProvider.Application.Interfaces.Repositories;
using Mock.ShippingProvider.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Infrastructure.Persistence.Repositories
{
    public class Repository<T>(ShippingProviderDbContext dbContext) : IRepository<T> where T : BaseEntity
    {
        private readonly ShippingProviderDbContext _dbContext = dbContext;
        private readonly DbSet<T> _dbSet = dbContext.Set<T>();

        public DbSet<T> Table => _dbSet;
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool isTrackingActive = true, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.Where(predicate);

            if (!isTrackingActive)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool isTrackingActive = true, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            if (!isTrackingActive)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(Guid id, bool isTrackingActive = true, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            if (!isTrackingActive)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstAsync(x => x.Id == id, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
