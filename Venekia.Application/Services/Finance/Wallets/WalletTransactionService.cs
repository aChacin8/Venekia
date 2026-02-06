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

        public async Task RegisterTransactionAsync (Wallet wallet, WalletTransaction.TransactionType type, decimal amount, decimal balanceBefore, decimal balanceAfter, string reference)
        {
            var transaction = new WalletTransaction(
                wallet.Id,
                type,
                amount,
                balanceBefore,
                balanceAfter,
                reference
                );

            await _transactionsRepository.AddAsyncTransaction(transaction);
        }

        public async Task <List<WalletTransaction>> GetAllTransactionsAsync(Guid walletId)
        {
            return await _transactionsRepository.GetTransactionsAsync(walletId);
        }

        public async Task<WalletTransaction?> GetOneTransactionByIdAsync(Guid walletId, Guid transactionId)
        {
            return await _transactionsRepository.GetTransactionByWalletAndIdAsync(walletId, transactionId);
        }
    }
}
