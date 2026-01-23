using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Venekia.Domain.Entities.Finance.Transactions;

namespace Venekia.Infrastructure.Data.Finance
{
    public class TransactionConfig : IEntityTypeConfiguration<Transaction>
    {
        public void Configure (EntityTypeBuilder<Transaction> entity)
        {
            entity.ToTable("Transactions", "dbo");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).HasColumnName("Id").HasDefaultValueSql("NEWID()");
            entity.HasOne(t => t.Wallet).WithOne(w => w.Transaction).HasForeignKey<Transaction>(t => t.Id).OnDelete(DeleteBehavior.Cascade);
            entity.Property(t => t.Type).HasColumnName("Type").IsRequired();
            entity.Property(t => t.Amount).HasColumnName("Amount").IsRequired();
            entity.Property(t => t.BalanceBefore).HasColumnName("BalanceBefore").IsRequired();
            entity.Property(t => t.BalanceAfter).HasColumnName("BalanceAfter").IsRequired();
            entity.Property(t => t.Reference).HasColumnName("Reference");
            entity.Property(w => w.CreatedAt).HasColumnName("CreatedAt").HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
