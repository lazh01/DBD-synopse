using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Crud.Infrastructure.Db
{
    public class CrudDbContextFactory : IDesignTimeDbContextFactory<CrudDbContext>
    {
        public CrudDbContext CreateDbContext(string[] args)
        {
            // Load configuration (appsettings.json)
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            // Build options
            var optionsBuilder = new DbContextOptionsBuilder<CrudDbContext>();
            var connectionString = configuration.GetConnectionString("WriteConnection")
                                   ?? "Host=localhost;Port=5434;Database=crud_db;Username=postgres;Password=postgres";

            optionsBuilder.UseNpgsql(connectionString);

            return new CrudDbContext(optionsBuilder.Options);
        }
    }
}