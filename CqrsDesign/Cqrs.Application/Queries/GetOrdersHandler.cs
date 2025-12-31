using Cqrs.Domain.Entities;
using Cqrs.Infrastructure.Db;

namespace Cqrs.Application.Queries;

using MongoDB.Driver;

public class GetOrdersHandler
{
    private readonly ReadDb _read;
    public GetOrdersHandler(ReadDb read) => _read = read;

    public async Task<List<Order>> Handle()
    {
        return await _read.Orders
            .Find(FilterDefinition<Order>.Empty)
            .ToListAsync();
    }
}