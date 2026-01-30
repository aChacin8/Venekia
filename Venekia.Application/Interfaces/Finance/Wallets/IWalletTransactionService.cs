using Venekia.Domain.Entities.Finance.Wallets;

namespace Venekia.Application.Interfaces.Finance.Wallets
{
    public interface IWalletTransactionService
    {
        Task RegisterCreditAsync(Wallet wallet, decimal amount, string reference);
        Task RegisterDebitAsync (Wallet wallet, decimal amount, string reference);
        Task<List<WalletTransaction>>  GetTransactionHistoryAsync(Guid walletId);
    }
}
