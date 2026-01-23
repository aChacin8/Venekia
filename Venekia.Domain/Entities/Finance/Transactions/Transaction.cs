using Venekia.Domain.Entities.Finance.Wallets;

namespace Venekia.Domain.Entities.Finance.Transactions
{
    public class Transaction
    {
        public Guid Id { get; private set; }
        public Guid WalletId { get; private set; }
        public Wallet Wallet { get; private set; } = null!;
        public TransactionType Type { get; private set; }
        public decimal Amount { get; private set; }
        public decimal BalanceBefore { get; private set; }
        public decimal BalanceAfter { get; private set; }
        public string Reference { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }

        public Transaction() { }

        public Transaction(Guid walletId, TransactionType type, decimal amount, decimal balanceBefore, decimal balanceAfter, string reference)
        {
            ValidateAmount(amount);
            ValidateReference(reference);

            Id = Guid.NewGuid();
            WalletId = walletId;
            Type = type;
            Amount = amount;
            BalanceBefore = balanceBefore;
            BalanceAfter = balanceAfter;
            Reference = reference;
            CreatedAt = DateTime.UtcNow;
        }
        public enum TransactionType
        {
            Credit,
            Debit
        }

        public void ValidateAmount (decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        }

        public void ValidateReference (string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
                throw new ArgumentException("Reference is required.", nameof(reference));
            if (reference.Length > 100)
                throw new ArgumentException("Reference cannot exceed 100 characters.", nameof(reference));
        }

    }
}
