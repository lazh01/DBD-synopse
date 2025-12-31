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

        await _read.Orders.InsertOneAsync(order);

        return order.Id;
    }
}