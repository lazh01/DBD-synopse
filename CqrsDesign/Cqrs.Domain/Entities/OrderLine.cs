using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cqrs.Domain.Entities;

[Table("order_lines")]
public class OrderLine
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey(nameof(Order))]
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;

    [ForeignKey(nameof(Product))]
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }

    // Add unit price at the time of order
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }
}