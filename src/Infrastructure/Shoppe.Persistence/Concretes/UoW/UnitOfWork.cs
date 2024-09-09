using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shoppe.Application.Abstractions.Repositories;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Persistence.Context;
using System;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.UoW
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ShoppeDbContext _context;
        private IDbContextTransaction? _transaction;
        private bool _disposed = false;

        // Entity-specific repositories
        public IProductReadRepository ProductReadRepository { get; private set; }
        public IProductWriteRepository ProductWriteRepository { get; private set; }

        public UnitOfWork(
            ShoppeDbContext context,
            IProductReadRepository productReadRepository,
            IProductWriteRepository productWriteRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            ProductReadRepository = productReadRepository ?? throw new ArgumentNullException(nameof(productReadRepository));
            ProductWriteRepository = productWriteRepository ?? throw new ArgumentNullException(nameof(productWriteRepository));
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        // Dispose pattern to clean up resources
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
