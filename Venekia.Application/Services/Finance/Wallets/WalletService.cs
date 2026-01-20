using Venekia.Application.DTOs.Finance.Wallet;
using Venekia.Application.Interfaces.Finance.Wallets;
using Venekia.Domain.Entities.Finance.Wallets;

namespace Venekia.Application.Services.Finance.Wallets
{
    public class WalletService
    {
        private readonly IWalletRepository _walletRepository;

        public WalletService (IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
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

        public async Task<List<WalletResponseDto>> GetWalletByUserAsync(Guid userId)
        {
            var existingWallet = await _walletRepository.GetWalletsByUserIdAsync(userId);
            if (!existingWallet.Any())
                throw new Exception("No wallets found for this user.");

            var wallet = existingWallet.Select(wallet => new WalletResponseDto
            {
                Id = wallet.Id,
                UserId = wallet.UserId,
                Balance = wallet.Balance,
                Currency = wallet.Currency,
                Status = wallet.Status.ToString()
            }).ToList();

            return wallet;
        }

        public async Task CreditAsync (Guid userId, CreditWalletDto creditWalletDto)
        {
            var wallet = await _walletRepository.GetByUserIdAndCurrencyAsync(userId, creditWalletDto.Currency);
            if (wallet == null)
                throw new InvalidOperationException("Wallet not found for the specified user and currency.");

            if (creditWalletDto.Amount <= 0)
                throw new Exception("Amount must be greater than zero.");

            wallet.Credit(creditWalletDto.Amount);

            await _walletRepository.UpdateAsyncWallet(wallet);
        }

        public async Task DebitAsync (Guid userId, DebitWalletDto debitWalletDto)
        {
            var wallet = await _walletRepository.GetByUserIdAndCurrencyAsync(userId, debitWalletDto.Currency);
            if (wallet == null)
                throw new Exception("Wallet not found for the specified user and currency.");

            if (debitWalletDto.Amount <= 0)
                throw new Exception("Amount must be greater than zero.");

            wallet.Debit(debitWalletDto.Amount);

            await _walletRepository.UpdateAsyncWallet(wallet);
        }
    }
}
