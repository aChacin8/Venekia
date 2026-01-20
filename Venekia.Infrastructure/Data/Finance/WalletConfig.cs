using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Venekia.Domain.Entities.Finance.Wallets;
using Venekia.Domain.Entities.Users;

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
            entity.HasOne<User>().WithOne(u => u.Wallet).HasForeignKey<Wallet>(w => w.UserId).OnDelete(DeleteBehavior.Cascade);

            entity.Property(w => w.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
            entity.Property(w => w.Balance).HasColumnName("Balance").HasDefaultValue(0m).IsRequired();
            entity.Property(w => w.Status).HasColumnName("Status").IsRequired();
            entity.Property(u => u.CreatedAt).HasColumnName("CreatedAt").HasDefaultValueSql("GETUTCDATE()");
            entity.Property(u => u.UpdatedAt).HasColumnName("UpdatedAt").HasDefaultValueSql("GETUTCDATE()");
        }
    }
}

