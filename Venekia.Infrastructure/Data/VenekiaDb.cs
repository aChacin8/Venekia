using Microsoft.EntityFrameworkCore;
using Venekia.Domain.Entities.Users;
using Venekia.Infrastructure.Data.Users;

namespace Venekia.Infrastructure.Data
{
    public class VenekiaDb : DbContext
    {
        public VenekiaDb(DbContextOptions<VenekiaDb> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>(); // Mapea en la entidad de EfCore de User.

        protected override void OnModelCreating(ModelBuilder modelBuilder) // Configura las entidades usando las configuraciones definidas.
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VenekiaDb).Assembly); // Aplica todas las configuraciones que tengan IEntityTypeConfiguration en el ensamblado actual.
        }
    }
}