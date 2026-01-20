namespace Venekia.Application.DTOs.Finance.Wallet
{
    public class DebitWalletDto
    {
        public string Currency { get; set; } = null!;
        public decimal Amount { get; set; }
    }
}