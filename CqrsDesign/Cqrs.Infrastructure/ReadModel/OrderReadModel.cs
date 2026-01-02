using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cqrs.Infrastructure.ReadModels;

public class OrderReadModel
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public UserReadModel User { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderLineReadModel> Lines { get; set; } = new();
}