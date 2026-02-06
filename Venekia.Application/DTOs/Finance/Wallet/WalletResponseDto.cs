namespace Venekia.Application.DTOs.Finance.Wallet
{
    public class WalletResponseDto
    {
        public Guid WalletId { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; } = null!;
        public string Status { get; set; } = null!;
    }

    public class WalletTransactionResponseDto
    {
        public Guid TransactionId { get; set; }
        public Guid WalletId { get; set; }
        public string Type { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public string Reference { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
