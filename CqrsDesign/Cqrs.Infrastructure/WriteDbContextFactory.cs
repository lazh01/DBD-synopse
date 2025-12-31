using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Cqrs.Infrastructure.Db
{
    public class WriteDbContextFactory : IDesignTimeDbContextFactory<WriteDbContext>
    {
        public WriteDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<WriteDbContext>();
            var connectionString = configuration.GetConnectionString("WriteConnection")
                                   ?? "Host=localhost;Port=5432;Database=cqrs_write;Username=postgres;Password=postgres";

            optionsBuilder.UseNpgsql(connectionString);

            return new WriteDbContext(optionsBuilder.Options);
        }
    }
}