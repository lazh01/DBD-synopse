namespace Crud.Application.Dtos;

public class OrderReadDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserReadDto User { get; set; } = null!;
    public List<OrderLineReadDto> Lines { get; set; } = new();
    public decimal TotalAmount { get; set; }
}