using Cqrs.Application.Commands;
using Cqrs.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Cqrs.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly CreateOrderHandler _createHandler;
    private readonly GetOrdersHandler _readHandler;

    public OrdersController(CreateOrderHandler createHandler, GetOrdersHandler readHandler)
    {
        _createHandler = createHandler;
        _readHandler = readHandler;
    }

    // POST /api/orders
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand cmd)
    {
        var id = await _createHandler.Handle(cmd);
        return Ok(new { OrderId = id });
    }

    // GET /api/orders
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _readHandler.Handle();
        return Ok(orders);
    }
}