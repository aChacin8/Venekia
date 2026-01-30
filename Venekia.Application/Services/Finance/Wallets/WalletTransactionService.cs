using Venekia.Application.Interfaces.Finance.Wallets;
using Venekia.Domain.Entities.Finance.Wallets;

namespace Venekia.Application.Services.Finance.Wallets
{
    public class WalletTransactionService : IWalletTransactionService
    {
        private readonly IWalletTransactionsRepository _transactionsRepository;

        public WalletTransactionService (IWalletTransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        public async Task RegisterCreditAsync (Wallet wallet, decimal amount, string reference)
        {
            var balanceBefore = wallet.Balance;

            wallet.Credit(amount);

            var balanceAfter = wallet.Balance;

            var transaction = new WalletTransaction(
                wallet.Id,
                WalletTransaction.TransactionType.Credit,
                amount,
                balanceBefore,
                balanceAfter,
                reference
                );

            await _transactionsRepository.AddAsyncTransaction(transaction);
        }

        public async Task RegisterDebitAsync (Wallet wallet, decimal amount, string reference)
        {
            var balanceBefore = wallet.Balance;

            wallet.Debit(amount);

            var balanceAfter = wallet.Balance;

            var transaction = new WalletTransaction(
                wallet.Id,
                WalletTransaction.TransactionType.Debit,
                amount,
                balanceBefore,
                balanceAfter,
                reference
                );

            await _transactionsRepository.AddAsyncTransaction(transaction);
        }

        public async Task <List<WalletTransaction>> GetTransactionHistoryAsync(Guid walletId)
        {
            return await _transactionsRepository.GetTransactionsAsync(walletId);
        }
    }
}
