namespace Cqrs.Application.Commands;

public class CreateOrderCommand
{
    public string Customer { get; set; } = "";
    public decimal Amount { get; set; }
}