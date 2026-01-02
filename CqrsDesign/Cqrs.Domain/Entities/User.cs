using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cqrs.Domain.Entities;

[Table("users")]
public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Name { get; set; } = "";

    [Required]
    public string Email { get; set; } = "";

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}