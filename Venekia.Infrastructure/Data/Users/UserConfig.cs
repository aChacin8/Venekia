using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Venekia.Domain.Entities.Users;

namespace Venekia.Infrastructure.Data.Users
{
    public class UserConfig : IEntityTypeConfiguration<User> // Configuracion de la entidad User para Entity Framework Core.
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Users", "dbo");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).HasColumnName("Id").ValueGeneratedNever();
            entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100).HasColumnName("FirstName");
            entity.Property(u => u.LastName).IsRequired().HasMaxLength(100).HasColumnName("LastName");
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100).HasColumnName("Email");
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(250).HasColumnName("Password");
            entity.Property(u => u.Address).HasMaxLength(250).HasColumnName("Address");
            entity.Property(u => u.PhoneNumber).HasMaxLength(250).HasColumnName("Phone");
            entity.Property(u => u.Status).IsRequired().HasColumnName("Status");
            entity.Property(u => u.CreatedAt).HasColumnName("CreatedAt").HasDefaultValueSql("GETUTCDATE()");
            entity.Property(u => u.UpdatedAt).HasColumnName("UpdatedAt").HasDefaultValueSql("GETUTCDATE()");
            entity.Property(u => u.DeactivatedAt).HasColumnName("DeactivatedAt");
        }
    }
}
