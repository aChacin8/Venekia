using Microsoft.EntityFrameworkCore;
using Venekia.Application.Interfaces.Finance.Wallets;
using Venekia.Domain.Entities.Finance.Wallets;
using Venekia.Infrastructure.Data;

namespace Venekia.Infrastructure.Repositories.Finance.Wallets
{
    public class WalletRepository : IWalletRepository
    {
        private readonly VenekiaDb _context;
        public WalletRepository(VenekiaDb context)
        {
            _context = context;
        }

        public async Task AddAsyncWallet(Wallet wallet)
        {
            await _context.Wallets.AddAsync(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Wallet wallet) {
            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Wallet>> GetWalletsByUserIdAsync(Guid userId)
        {
            return await _context.Wallets
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }
        public async Task<Wallet?> GetByUserIdAndCurrencyAsync(Guid userId, string currency)
        {
            return await _context.Wallets
                .FirstOrDefaultAsync(w => w.UserId == userId && w.Currency == currency);
        }
    }
}
