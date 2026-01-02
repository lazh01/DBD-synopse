using Cqrs.Domain.Events;
using Cqrs.Infrastructure.Db;
using Cqrs.Infrastructure.ReadModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
namespace Cqrs.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class ReadModelUpdater : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ReadModelUpdater(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("ReadModelUpdater started.");
        await foreach (var evt in DomainEvents.Queue.Reader.ReadAllAsync(stoppingToken))
        {
            Console.WriteLine(evt);
            if (evt is OrderCreatedEvent e)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var write = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
                    var read = scope.ServiceProvider.GetRequiredService<ReadDb>();

                    // Hent order med navigation properties
                    var order = await write.Orders
                        .Include(o => o.User)
                        .Include(o => o.Lines)
                            .ThenInclude(l => l.Product)
                        .FirstAsync(o => o.Id == e.OrderId, stoppingToken);

                    var readOrder = new OrderReadModel
                    {
                        Id = order.Id,
                        CreatedAt = order.CreatedAt,
                        User = new UserReadModel
                        {
                            Id = order.User.Id,
                            Name = order.User.Name,
                            Email = order.User.Email
                        },
                        Lines = order.Lines.Select(l => new OrderLineReadModel
                        {
                            Id = l.Id,
                            Quantity = l.Quantity,
                            Product = new ProductReadModel
                            {
                                Id = l.Product.Id,
                                Name = l.Product.Name,
                                UnitPrice = l.UnitPrice
                            }
                        }).ToList(),
                        TotalAmount = order.Lines.Sum(l => l.Quantity * l.UnitPrice)
                    };

                    await read.Orders.InsertOneAsync(readOrder);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fejl i ReadModelUpdater: {ex.Message}");
                    // evt. log eller retry mekanisme
                }
            }
        }
    }
}

public static class DomainEvents
{
    public static Channel<object> Queue { get; } = Channel.CreateUnbounded<object>();
}