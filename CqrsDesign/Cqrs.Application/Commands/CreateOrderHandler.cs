using Cqrs.Domain.Entities;
using Cqrs.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Cqrs.Application.Commands;

public class CreateOrderHandler
{
    private readonly WriteDbContext _write;
    private readonly ReadDb _read;

    public CreateOrderHandler(WriteDbContext write, ReadDb read)
    {
        _write = write;
        _read = read;
    }

    public async Task<Guid> Handle(CreateOrderCommand cmd)
    {
        var order = new Order { Customer = cmd.Customer, Amount = cmd.Amount };

        _write.Orders.Add(order);
        await _write.SaveChangesAsync();

        // Fire-and-forget of read db
        // Vi forholder os simpelt da vi bare vil have noget der ville være representabelt af en cqrs struktur
        // I et rigtigt system ville vi lave et system med service for read db og service for write db, samt kommunikation imellem.
        // Derved ville der kunne være koordinering af opdateringer, og skabe en mere robust løsning.
        _ = Task.Run(async () =>
        {
            try
            {
                await _read.Orders.InsertOneAsync(order);
            }
            catch (Exception ex)
            {
                // Log fejl, retry hvis nødvendigt
                Console.WriteLine($"Fejl ved opdatering af read DB: {ex.Message}");
            }
        });

        return order.Id;
    }
}