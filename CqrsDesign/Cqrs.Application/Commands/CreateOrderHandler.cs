using Cqrs.Application.Services;
using Cqrs.Domain.Entities;
using Cqrs.Domain.Events;
using Cqrs.Infrastructure.Db;
using Cqrs.Infrastructure.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Cqrs.Application.Commands;

public class CreateOrderHandler
{
    private readonly WriteDbContext _write;

    public CreateOrderHandler(WriteDbContext write)
    {
        _write = write;
    }

    public async Task<Guid> Handle(CreateOrderCommand cmd)
    {
        var order = new Order
        {
            UserId = cmd.UserId,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var line in cmd.Lines)
        {
            var product = await _write.Products.FindAsync(line.ProductId)
                          ?? throw new Exception($"Product {line.ProductId} not found");

            order.Lines.Add(new OrderLine
            {
                ProductId = product.Id,
                Quantity = line.Quantity,
                UnitPrice = product.UnitPrice
            });
        }

        _write.Orders.Add(order);
        await _write.SaveChangesAsync();

        // Udløs event – alle subscribers læser fra channel
        await DomainEvents.Queue.Writer.WriteAsync(new OrderCreatedEvent(order.Id));

        return order.Id;
    }
}