using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Venekia.Domain.Entities.Finance.Wallets;

namespace Venekia.Infrastructure.Data.Finance
{
    public class WalletConfig : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> entity)
        {
            entity.ToTable("Wallets", "dbo");
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Id).HasColumnName("Id").ValueGeneratedNever();
            entity.Property(w => w.UserId).HasColumnName("UserId").IsRequired();
            entity.HasOne(w => w.User).WithOne(u => u.Wallet).HasForeignKey<Wallet>(w => w.UserId).OnDelete(DeleteBehavior.Cascade);

            entity.Property(w => w.Currency).HasColumnName("Currency").IsRequired();
            entity.Property(w => w.Balance).HasColumnName("Balance").IsRequired().HasDefaultValue(0.00);
            entity.Property(w => w.Status).HasColumnName("Status").IsRequired();
            entity.Property(u => u.CreatedAt).HasColumnName("CreatedAt").HasDefaultValueSql("GETUTCDATE()");
            entity.Property(u => u.UpdatedAt).HasColumnName("UpdatedAt").HasDefaultValueSql("GETUTCDATE()");
        }
    }
}

