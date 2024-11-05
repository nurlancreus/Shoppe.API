using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories;
using Shoppe.Domain.Entities.Base;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories
{
    public class ReadRepository<T> : Repository<T>, IReadRepository<T> where T : BaseEntity
    {
        public ReadRepository(ShoppeDbContext context) : base(context)
        {
        }

        public async Task<IQueryable<T>> GetAllAsync( bool isTracking = true)
        {
            var query = Table.AsQueryable();

            if (!isTracking) query.AsNoTracking();

            return await Task.FromResult(query);
        }

        public async Task<IQueryable<T>> GetAllWhereAsync(Expression<Func<T, bool>> method, bool isTracking = true)
        {
            var query = Table.Where(method).AsQueryable();

            if (!isTracking) query.AsNoTracking();

            return await Task.FromResult(query);
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> method, CancellationToken cancellationToken, bool isTracking = true)
        {
            var query = Table.AsQueryable();

            if (!isTracking) query.AsNoTracking();

            return await query.FirstOrDefaultAsync(method, cancellationToken);

        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool isTracking = true)
        {

            var query = Table.AsQueryable();

            if (!isTracking) query.AsNoTracking();

            return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<bool> IsExistAsync(Expression<Func<T, bool>> method, CancellationToken cancellationToken)
        {
            return await Table.AnyAsync(method, cancellationToken);
        }
    }
}
