using Common.Core.Model.NoSQL;

namespace Common.Core.Interface.NoSQL;

/// <summary>
/// Azure Cosmos DB
/// </summary>
public interface ICosmosContext
{
    Task CreateAsync(EventHistory entity);
    Task CreateManyAsync(IEnumerable<EventHistory> entity);
    Task<EventHistory> GetAsync(string transactionId, string entity);
}