using Cqrs.Domain.Entities;
using Cqrs.Infrastructure.Db;
using Cqrs.Infrastructure.ReadModels;
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
        var order = new Order
        {
            UserId = cmd.UserId,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var line in cmd.Lines)
        {
            var product = await _write.Products.FindAsync(line.ProductId);
            if (product == null) throw new Exception($"Product {line.ProductId} not found");

            order.Lines.Add(new OrderLine
            {
                ProductId = product.Id,
                Quantity = line.Quantity,
                UnitPrice = product.UnitPrice
            });
        }

        _write.Orders.Add(order);
        await _write.SaveChangesAsync();

        // Populate read DB immediately (synchronously, safe)
        var user = await _write.Users.FirstOrDefaultAsync(u => u.Id == order.UserId)
                   ?? throw new Exception($"User {order.UserId} not found");

        var readLines = new List<OrderLineReadModel>();
        foreach (var line in order.Lines)
        {
            var product = await _write.Products.FirstOrDefaultAsync(p => p.Id == line.ProductId)
                          ?? throw new Exception($"Product {line.ProductId} not found");

            readLines.Add(new OrderLineReadModel
            {
                Id = line.Id,
                Quantity = line.Quantity,
                Product = new ProductReadModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    UnitPrice = product.UnitPrice
                }
            });
        }

        var readOrder = new OrderReadModel
        {
            Id = order.Id,
            CreatedAt = order.CreatedAt,
            User = new UserReadModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            },
            Lines = readLines,
            TotalAmount = readLines.Sum(l => l.Quantity * l.Product.UnitPrice)
        };

        await _read.Orders.InsertOneAsync(readOrder);

        return order.Id;
    }
}