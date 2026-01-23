using Venekia.Application.DTOs.Finance.Wallet;

namespace Venekia.Application.Interfaces.Finance.Wallets
{
    public interface IWalletService
    {
        Task<WalletResponseDto> CreateWalletAsync(Guid userId, CreateWalletDto createWalletDto);
        Task<List<WalletResponseDto>> GetWalletByUserAsync(Guid userId);
        Task<WalletResponseDto> CreditAsync(Guid userId, CreditWalletDto creditWalletDto);
        Task<WalletResponseDto> DebitAsync(Guid userId, DebitWalletDto debitWalletDto);
    }
}
  