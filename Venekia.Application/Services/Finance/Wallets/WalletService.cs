using Venekia.Application.DTOs.Finance.Wallet;
using Venekia.Application.Interfaces.Common;
using Venekia.Application.Interfaces.Finance.Wallets;
using Venekia.Domain.Entities.Finance.Wallets;

namespace Venekia.Application.Services.Finance.Wallets
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletTransactionService _transactionService;
        private readonly IUnitOfWork _unitOfWork;

        public WalletService(
            IWalletRepository walletRepository, IWalletTransactionService transactionService,IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _transactionService = transactionService;
            _unitOfWork = unitOfWork;
        }

        public async Task<WalletResponseDto> CreateWalletAsync(Guid userId, CreateWalletDto createWalletDto)
        {
            var existingWallet = await _walletRepository.GetByUserIdAndCurrencyAsync(userId, createWalletDto.Currency);

            if (existingWallet != null)
                throw new InvalidOperationException("Wallet with the specified currency already exists for this user.");

            var wallet = new Wallet(userId, createWalletDto.Currency);

            await _walletRepository.AddAsyncWallet(wallet);

            return MapToWalletResponseDto(wallet);
        }

        public async Task<WalletResponseDto> CreditAsync(Guid userId, CreditWalletDto creditWalletDto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var wallet = await _walletRepository.GetByUserIdAndCurrencyAsync(userId, creditWalletDto.Currency);

                if (wallet is null)
                    throw new InvalidOperationException("Wallet not found for the specified user and currency.");

                ValidateAmount(creditWalletDto.Amount);

                var balanceBefore = wallet.Balance;

                wallet.Credit(creditWalletDto.Amount);

                var balanceAfter = wallet.Balance;

                await _walletRepository.UpdateAsyncWallet(wallet);

                await _transactionService.RegisterTransactionAsync(
                    wallet,
                    WalletTransaction.TransactionType.Credit,
                    creditWalletDto.Amount,
                    balanceBefore,
                    balanceAfter,
                    $"Credit: {creditWalletDto.Currency}"
                );

                await _unitOfWork.CommitAsync();

                return MapToWalletResponseDto(wallet);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<WalletResponseDto> DebitAsync(Guid userId, DebitWalletDto debitWalletDto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var wallet = await _walletRepository.GetByUserIdAndCurrencyAsync(userId, debitWalletDto.Currency);

                if (wallet is null)
                    throw new InvalidOperationException("Wallet not found for the specified user and currency.");

                ValidateAmount(debitWalletDto.Amount);

                var balanceBefore = wallet.Balance;

                wallet.Debit(debitWalletDto.Amount);

                var balanceAfter = wallet.Balance;

                await _walletRepository.UpdateAsyncWallet(wallet);

                await _transactionService.RegisterTransactionAsync(
                    wallet,
                    WalletTransaction.TransactionType.Debit,
                    debitWalletDto.Amount,
                    balanceBefore,
                    balanceAfter,
                    $"Debit: {debitWalletDto.Currency}"
                );

                await _unitOfWork.CommitAsync();

                return MapToWalletResponseDto(wallet);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<List<WalletResponseDto>> GetWalletByUserAsync(Guid userId)
        {
            var wallets = await _walletRepository.GetWalletsByUserIdAsync(userId);

            if (wallets is null || wallets.Count == 0)
                throw new InvalidOperationException("No wallets found for this user.");

            return wallets.Select(MapToWalletResponseDto).ToList();
        }

        public async Task<List<WalletTransactionResponseDto>> GetTransactionHistoryAsync(Guid userId, Guid walletId)
        {
            var wallets = await _walletRepository.GetWalletsByUserIdAsync(userId);

            var wallet = wallets.FirstOrDefault(w => w.Id == walletId);

            if (wallet is null)
                throw new InvalidOperationException("Wallet not found for this user.");

            var transactions = await _transactionService.GetAllTransactionsAsync(walletId);

            return transactions.Select(MapToWalletTransaction).ToList();
        }

        public async Task<WalletTransactionResponseDto> GetTransactionByIdAsync(Guid userId, Guid walletId, Guid transactionId)
        {
            var wallets = await _walletRepository.GetWalletsByUserIdAsync(userId);

            var wallet = wallets.FirstOrDefault(w => w.Id == walletId);

            if (wallet is null)
                throw new InvalidOperationException("Wallet not found for this user.");

            var transaction = await _transactionService.GetOneTransactionByIdAsync(walletId, transactionId);

            if (transaction is null)
                throw new InvalidOperationException("Transaction not found.");

            return MapToWalletTransaction(transaction);
        }

        private static void ValidateAmount(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        }

        private static WalletResponseDto MapToWalletResponseDto(Wallet wallet)
        {
            return new WalletResponseDto
            {
                WalletId = wallet.Id,
                UserId = wallet.UserId,
                Balance = wallet.Balance,
                Currency = wallet.Currency,
                Status = wallet.Status.ToString()
            };
        }

        private static WalletTransactionResponseDto MapToWalletTransaction(WalletTransaction transaction)
        {
            return new WalletTransactionResponseDto
            {
                TransactionId = transaction.Id,
                WalletId = transaction.WalletId,
                Type = transaction.Type.ToString(),
                Amount = transaction.Amount,
                BalanceBefore = transaction.BalanceBefore,
                BalanceAfter = transaction.BalanceAfter,
                Reference = transaction.Reference,
                CreatedAt = transaction.CreatedAt
            };
        }
    }
}
