using Venekia.Application.DTOs.Finance.Wallet;

namespace Venekia.Application.Interfaces.Finance.Wallets
{
    public interface IWalletService
    {
        Task<WalletResponseDto> CreateWalletAsync(Guid userId, CreateWalletDto createWalletDto);
        Task<WalletResponseDto> CreditAsync(Guid userId, CreditWalletDto creditWalletDto);
        Task<WalletResponseDto> DebitAsync(Guid userId, DebitWalletDto debitWalletDto);
        Task<List<WalletResponseDto>> GetWalletByUserAsync(Guid userId);
        Task<List<WalletTransactionResponseDto>> GetTransactionHistoryAsync(Guid userId, Guid walletId);
        Task<WalletTransactionResponseDto> GetTransactionByIdAsync(Guid userId, Guid walletId, Guid transactionId);
    }
}
  