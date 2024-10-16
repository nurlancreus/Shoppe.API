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
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
