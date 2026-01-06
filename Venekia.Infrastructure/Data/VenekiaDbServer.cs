using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

namespace Venekia.Infrastructure.Data
{
    public class VenekiaDbFactory : IDesignTimeDbContextFactory<VenekiaDb>
    {
        public VenekiaDb CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<VenekiaDb>();

            optionsBuilder.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")
                ?? "Server=localhost;Database=VenekiaDb;Trusted_Connection=True;TrustServerCertificate=True"
            );

            return new VenekiaDb(optionsBuilder.Options);
        }
    }
}
