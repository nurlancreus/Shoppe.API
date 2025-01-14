using Microsoft.EntityFrameworkCore;
using Mock.ShippingProvider.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> Table { get; }
        Task<IEnumerable<T>> GetAllAsync(bool isTrackingActive = true, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(Guid id, bool isTrackingActive = true, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool isTrackingActive = true, CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
