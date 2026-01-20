using Venekia.Domain.Entities.Finance.Wallets;
using Venekia.Domain.Entities.Users;

namespace Venekia.Application.Interfaces.Finance.Wallets
{
    public interface IWalletRepository
    {
        Task AddAsyncWallet(Wallet wallet);
        Task UpdateAsync(Wallet wallet);    
        Task DeleteAsync(Wallet wallet);
        Task <List<Wallet>> GetWalletsByUserIdAsync(Guid userId);
        Task<Wallet?> GetByUserIdAndCurrencyAsync(Guid userId, string currency);
    }
}
