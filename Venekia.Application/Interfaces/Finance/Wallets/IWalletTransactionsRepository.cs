using Venekia.Domain.Entities.Finance.Wallets;

namespace Venekia.Application.Interfaces.Finance.Wallets
{
    public interface IWalletTransactionsRepository
    {
        Task AddAsyncTransaction(WalletTransaction transaction);
        Task<List<WalletTransaction>> GetTransactionsAsync(Guid walletId);
        Task<WalletTransaction?> GetTransactionByIdAsync(Guid transactionId);
    }
}
