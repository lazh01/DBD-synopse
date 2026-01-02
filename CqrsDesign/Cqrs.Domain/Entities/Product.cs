using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cqrs.Domain.Entities;

[Table("products")]
public class Product
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Name { get; set; } = "";

    [Required]
    public decimal UnitPrice { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}