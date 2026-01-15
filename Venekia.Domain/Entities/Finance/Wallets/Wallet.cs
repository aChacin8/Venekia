using Venekia.Domain.Entities.Users;

namespace Venekia.Domain.Entities.Finance.Wallets
{
    public class Wallet
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Currency { get; set; } = null!;
        public decimal Balance { get; private set; } = 0.00m;
        public WalletStatus Status { get; set; } = WalletStatus.active;
        public User User { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public Wallet() {}

        public Wallet(Guid userId, string currency)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Currency = currency;
            Balance = 0m;
            Status = WalletStatus.active;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
