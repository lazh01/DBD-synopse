using Crud.Application.Dtos;
using Crud.Domain.Entities;
using Crud.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Crud.Application.Services;

public class CrudOrderService
{
    private readonly CrudDbContext _db;

    public CrudOrderService(CrudDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> CreateOrderAsync(CreateOrderDto dto)
    {
        var order = new Order
        {
            UserId = dto.UserId,
            CreatedAt = DateTime.UtcNow
        };

        var products = await _db.Products
            .Where(p => dto.Lines.Select(l => l.ProductId).Contains(p.Id))
            .ToListAsync();

        foreach (var line in dto.Lines)
        {
            var product = products.FirstOrDefault(p => p.Id == line.ProductId)
                          ?? throw new Exception($"Product {line.ProductId} not found");

            order.Lines.Add(new OrderLine
            {
                ProductId = product.Id,
                Quantity = line.Quantity,
                UnitPrice = product.UnitPrice
            });
        }

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return order.Id;
    }

    public async Task<List<OrderReadDto>> GetOrdersAsync()
    {
        return await _db.Orders
            .Include(o => o.User)
            .Include(o => o.Lines)
                .ThenInclude(l => l.Product)
            .Select(o => new OrderReadDto
            {
                Id = o.Id,
                CreatedAt = o.CreatedAt,
                User = new UserReadDto
                {
                    Id = o.User.Id,
                    Name = o.User.Name,
                    Email = o.User.Email
                },
                Lines = o.Lines.Select(l => new OrderLineReadDto
                {
                    ProductId = l.ProductId,
                    ProductName = l.Product.Name,
                    Quantity = l.Quantity,
                    UnitPrice = l.UnitPrice
                }).ToList(),
                TotalAmount = o.Lines.Sum(l => l.Quantity * l.UnitPrice)
            })
            .ToListAsync();
    }
}