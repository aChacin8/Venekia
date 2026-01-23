using Microsoft.EntityFrameworkCore.Storage;
using Venekia.Application.Interfaces.Common;
using Venekia.Infrastructure.Data;

namespace Venekia.Infrastructure.Repositories.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VenekiaDb _context;
        private IDbContextTransaction? _currentTransaction; 

        public UnitOfWork(VenekiaDb context) 
        {
            _context = context;
        } 

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null) return;

            _currentTransaction = await _context.Database.BeginTransactionAsync(); // Start a new transaction
        }
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            await _currentTransaction?.CommitAsync()!;
            await DisposeTransactionAsync();
        }

        public async Task RollbackAsync()
        {
            await _currentTransaction?.RollbackAsync()!; // Rollback the transaction
            await DisposeTransactionAsync();
        }
        
        private async Task DisposeTransactionAsync() // Dispose of the current transaction
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
