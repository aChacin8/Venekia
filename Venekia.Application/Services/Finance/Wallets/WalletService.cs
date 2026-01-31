using Venekia.Application.DTOs.Finance.Wallet;
using Venekia.Application.Interfaces.Common;
using Venekia.Application.Interfaces.Finance.Wallets;
using Venekia.Domain.Entities.Finance.Wallets;

namespace Venekia.Application.Services.Finance.Wallets
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWalletTransactionService _transactionService;

        public WalletService (IWalletRepository walletRepository, IUnitOfWork unitOfWork ,IWalletTransactionService transactionService)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
            _transactionService = transactionService;
        }

        public async Task <WalletResponseDto> CreateWalletAsync(Guid userId, CreateWalletDto createWalletDto)
        {
            var existingWallet = await _walletRepository.GetByUserIdAndCurrencyAsync(userId, createWalletDto.Currency);
            if(existingWallet != null)
                throw new InvalidOperationException("Wallet with the specified currency already exists for this user.");
            
            var wallet = new Wallet (userId, createWalletDto.Currency);

            await _walletRepository.AddAsyncWallet(wallet);

            return new WalletResponseDto
            {
                Id = wallet.Id,
                UserId = wallet.UserId,
                Balance = wallet.Balance,
                Currency = wallet.Currency,
                Status = wallet.Status.ToString()
            };
        }


        public async Task<WalletResponseDto> CreditAsync (Guid userId, CreditWalletDto creditWalletDto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var wallet = await _walletRepository.GetByUserIdAndCurrencyAsync(userId, creditWalletDto.Currency);
                if (wallet == null)
                    throw new InvalidOperationException("Wallet not found for the specified user and currency.");

                ValidateAmount(creditWalletDto.Amount);

                var beforeBalance = wallet.Balance;

                wallet.Credit(creditWalletDto.Amount);

                var afterBalance = wallet.Balance;

                await _walletRepository.UpdateAsyncWallet(wallet);
                await _transactionService.RegisterCreditAsync(wallet, creditWalletDto.Amount, beforeBalance, afterBalance, $"Credit: {creditWalletDto.Currency}");
                
                await _unitOfWork.CommitAsync();

                return MapToWalletResponseDto(wallet);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<WalletResponseDto> DebitAsync (Guid userId, DebitWalletDto debitWalletDto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var wallet = await _walletRepository.GetByUserIdAndCurrencyAsync(userId, debitWalletDto.Currency);
                if (wallet == null)
                    throw new InvalidOperationException("Wallet not found for the specified user and currency.");

                ValidateAmount(debitWalletDto.Amount);

                var balanceBefore = wallet.Balance;

                wallet.Debit(debitWalletDto.Amount);
                
                var balanceAfter = wallet.Balance;

                await _walletRepository.UpdateAsyncWallet(wallet);
                await _transactionService.RegisterDebitAsync(wallet, debitWalletDto.Amount, balanceBefore, balanceAfter, $"Debit: {debitWalletDto.Currency}");

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

            if (!wallets.Any())
                throw new InvalidOperationException("No wallets found.");

            return wallets.Select(MapToWalletResponseDto).ToList();
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
                Id = wallet.Id,
                UserId = wallet.UserId,
                Balance = wallet.Balance,
                Currency = wallet.Currency,
                Status = wallet.Status.ToString()
            };
        }
    }
}
