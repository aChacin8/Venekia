namespace Venekia.Application.DTOs.Finance.Wallet
{
    public class WalletResponseDto
    {
        Guid Id { get; set; }
        decimal Balance { get; set; }
        string Currency { get; set; } = null!;
        string Status { get; set; } = null!;
    }
}
