using Cqrs.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

public class ReadDb
{
    private readonly IMongoDatabase _db;

    public ReadDb(IConfiguration config)
    {
        var connectionString = config["ReadConnection"]; // now includes credentials
        var client = new MongoClient(connectionString);
        _db = client.GetDatabase("cqrs_read"); // your read DB
    }

    public IMongoCollection<Order> Orders => _db.GetCollection<Order>("Orders");
}