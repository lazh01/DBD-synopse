using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class ProductReadModel
{
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public decimal UnitPrice { get; set; }
}
