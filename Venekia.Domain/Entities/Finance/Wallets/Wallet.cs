using Venekia.Domain.Entities.Users;

namespace Venekia.Domain.Entities.Finance.Wallets
{
    public class Wallet
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Currency { get; private set; } = null!;
        public decimal Balance { get; private set; } = 0.00m;
        public WalletStatus Status { get; private set; } = WalletStatus.Active;
        public User User { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public Wallet() {}

        public Wallet(Guid userId, string currency)
        {
            ValidateCurrency(currency);

            Id = Guid.NewGuid();
            UserId = userId;
            Currency = currency.ToUpper();
            Balance = 0m;
            Status = WalletStatus.Active;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Credit(decimal amount) //Para agregar fondos a la billetera.
        {
            IsActive();
            
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

            Balance += amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Debit(decimal amount) //Para retirar fondos de la billetera.
        {
            IsActive();

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

            if (amount > Balance)
                throw new InvalidOperationException("Insufficient funds.");

            Balance -= amount;
            UpdatedAt = DateTime.UtcNow;
        }


        private void ValidateCurrency(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be null or empty.", nameof(currency));

            if(currency.Length != 3)
                throw new ArgumentException("Currency must be a 3-letter ISO code.", nameof(currency));
        }

        private void IsActive()
        {
            if (Status != WalletStatus.Active)
                throw new InvalidOperationException("Wallet is not active.");
        }

        public void Suspend()
        {
            Status = WalletStatus.Suspended;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            Status = WalletStatus.Inactive;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
