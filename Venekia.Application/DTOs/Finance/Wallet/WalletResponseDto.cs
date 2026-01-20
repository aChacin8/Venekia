namespace Venekia.Application.DTOs.Finance.Wallet
{
    public class WalletResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
