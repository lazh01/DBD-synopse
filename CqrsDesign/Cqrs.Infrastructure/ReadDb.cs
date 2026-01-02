using Cqrs.Infrastructure.ReadModels;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

public class ReadDb
{
    private readonly IMongoDatabase _db;

    public ReadDb(IConfiguration config)
    {
        var connectionString = config["ReadConnection"];
        var client = new MongoClient(connectionString);
        _db = client.GetDatabase("cqrs_read");
    }

    public IMongoCollection<OrderReadModel> Orders =>
        _db.GetCollection<OrderReadModel>("orders");
}