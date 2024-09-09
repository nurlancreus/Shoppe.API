using Microsoft.EntityFrameworkCore.Storage;
using Shoppe.Application.Abstractions.Repositories;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShoppeDbContext _context;
        //private IDbContextTransaction _transaction;
        private bool _disposed = false;

        // Entity-specific repositories
        public IProductReadRepository ProductReadRepository { get; private set; }
        public IProductWriteRepository ProductWriteRepository { get; private set; }

        public UnitOfWork(ShoppeDbContext context, IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _context = context;
            //_transaction = transaction;
            ProductReadRepository = productReadRepository;
            ProductWriteRepository = productWriteRepository;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {

            using var transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                //if (_transaction != null)
                //{
                //    await _transaction.CommitAsync();
                //}
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                //if (_transaction != null)
                //{
                //    await _transaction.DisposeAsync();
                //    _transaction = null;
                //}
            }
        }

        public async Task RollbackTransactionAsync()
        {
            //if (_transaction != null)
            //{
            //    await _transaction.RollbackAsync();
            //    await _transaction.DisposeAsync();
            //    _transaction = null;
            //}
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
