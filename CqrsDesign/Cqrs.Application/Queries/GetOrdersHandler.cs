using Cqrs.Infrastructure.ReadModels;
using MongoDB.Driver;

namespace Cqrs.Application.Queries;

public class GetOrdersHandler
{
    private readonly ReadDb _read;

    public GetOrdersHandler(ReadDb read)
    {
        _read = read;
    }

    public async Task<List<OrderReadModel>> Handle()
    {
        return await _read.Orders
            .Find(FilterDefinition<OrderReadModel>.Empty)
            .ToListAsync();
    }
}