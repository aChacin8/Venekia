using Venekia.Domain.Entities.Finance.Wallets;

namespace Venekia.Application.Interfaces.Finance.Wallets
{
    public interface IWalletTransactionService
    {
        Task RegisterTransactionAsync(Wallet wallet, WalletTransaction.TransactionType type,  decimal amount, decimal balanceBefore, decimal balanceAfter, string reference);
        Task<List<WalletTransaction>> GetAllTransactionsAsync(Guid walletId);
        Task<WalletTransaction?> GetOneTransactionByIdAsync(Guid walletId, Guid transactionId);
    }
}
