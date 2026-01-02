namespace Cqrs.Application.Commands;

public class CreateOrderCommand
{
    public Guid UserId { get; set; }

    public List<CreateOrderLineDto> Lines { get; set; } = new();
}

public class CreateOrderLineDto
{
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
}