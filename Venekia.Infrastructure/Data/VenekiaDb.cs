using Microsoft.EntityFrameworkCore;

using Venekia.Domain.Entities;

namespace Venekia.Infrastructure.Data
{
    public class VenekiaDb : DbContext
    {
        public VenekiaDb(DbContextOptions<VenekiaDb> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User> (entity =>
            {
                entity.ToTable("users");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).HasColumnName("user_id").ValueGeneratedNever();
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100).HasColumnName("first_name");
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(100).HasColumnName("last_name");
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100).HasColumnName("email");
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(250).HasColumnName("password");
                entity.Property(u => u.Address).HasMaxLength(250).HasColumnName("address");
                entity.Property(u => u.PhoneNumber).HasMaxLength(250).HasColumnName("phone");
            });
        }
    }
}