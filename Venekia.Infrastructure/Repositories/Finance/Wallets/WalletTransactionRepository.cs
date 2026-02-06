using Microsoft.EntityFrameworkCore;
using Venekia.Application.Interfaces.Finance.Wallets;
using Venekia.Domain.Entities.Finance.Wallets;
using Venekia.Infrastructure.Data;

namespace Venekia.Infrastructure.Repositories.Finance.Wallets
{
    public class WalletTransactionRepository : IWalletTransactionsRepository
    {
        private readonly VenekiaDb _context;

        public WalletTransactionRepository(VenekiaDb context)
        {
            _context = context;
        }

        public async Task AddAsyncTransaction(WalletTransaction transaction)
        {
            await _context.WTransactions.AddAsync(transaction);
        }

        public async Task<List<WalletTransaction>> GetTransactionsAsync(Guid walletId)
        {
            return await _context.WTransactions.Where(t => t.WalletId == walletId).OrderByDescending(t => t.CreatedAt).ToListAsync();
        }

        public async Task<WalletTransaction?> GetTransactionByWalletAndIdAsync(Guid walletId, Guid transactionId)
        {
            return await _context.WTransactions.FirstOrDefaultAsync(t => t.Id == transactionId && t.WalletId == walletId);
        }
    }
}
