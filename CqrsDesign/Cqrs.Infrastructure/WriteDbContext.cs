using Cqrs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cqrs.Infrastructure.Db;

public class WriteDbContext : DbContext
{
    public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options) { }
    public DbSet<Order> Orders => Set<Order>();
}