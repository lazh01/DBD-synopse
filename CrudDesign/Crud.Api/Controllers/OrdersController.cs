using Microsoft.AspNetCore.Mvc;
using Crud.Application.Services;
using Crud.Application.Dtos;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly CrudOrderService _service;

    public OrdersController(CrudOrderService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
    {
        var id = await _service.CreateOrderAsync(dto);
        return Ok(new { OrderId = id });
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _service.GetOrdersAsync();
        return Ok(orders);
    }
}