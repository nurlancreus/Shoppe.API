using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Shoppe.Application.Abstractions.Repositories;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Domain.Entities.Base;
using Shoppe.Persistence.Context;
using System;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShoppeDbContext _context;
        public UnitOfWork(ShoppeDbContext context)
        {
            _context = context;
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateDateTimesWhileSavingInterceptor();

            await _context.SaveChangesAsync(cancellationToken);
        }

        private void UpdateDateTimesWhileSavingInterceptor()
        {
            var changedEntries = _context.ChangeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

            foreach (var entry in changedEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
