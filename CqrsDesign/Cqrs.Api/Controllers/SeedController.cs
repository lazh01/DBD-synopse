using Cqrs.Domain.Entities;
using Cqrs.Infrastructure.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cqrs.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeedController : ControllerBase
{
    private readonly WriteDbContext _write;

    public SeedController(WriteDbContext write)
    {
        _write = write;
    }

    // DELETE /api/seed/clear
    [HttpDelete("clear")]
    public async Task<IActionResult> ClearDatabase()
    {
        // Ryd OrderLines først pga FK
        await _write.OrderLines.ExecuteDeleteAsync();
        await _write.Orders.ExecuteDeleteAsync();
        await _write.Users.ExecuteDeleteAsync();
        await _write.Products.ExecuteDeleteAsync();

        return Ok("Database cleared");
    }

    // POST /api/seed/users
    [HttpPost("users")]
    public async Task<IActionResult> SeedUsers()
    {
        var users = new List<User>();
        for (int i = 1; i <= 5; i++)
        {
            users.Add(new User { Name = $"User {i}", Email = $"user{i}@example.com" });
        }

        _write.Users.AddRange(users);
        await _write.SaveChangesAsync();

        return Ok(users);
    }

    // POST /api/seed/products
    [HttpPost("products")]
    public async Task<IActionResult> SeedProducts()
    {
        var products = new List<Product>();
        for (int i = 1; i <= 10; i++)
        {
            products.Add(new Product { Name = $"Product {i}", UnitPrice = 10m + i });
        }

        _write.Products.AddRange(products);
        await _write.SaveChangesAsync();

        return Ok(products);
    }
}