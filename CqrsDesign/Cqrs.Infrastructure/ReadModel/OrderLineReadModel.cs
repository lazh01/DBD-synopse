using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cqrs.Infrastructure.ReadModels;

public class OrderLineReadModel
{
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public ProductReadModel Product { get; set; } = null!;
    public int Quantity { get; set; }
}