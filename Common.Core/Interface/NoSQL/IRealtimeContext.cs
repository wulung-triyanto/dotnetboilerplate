using Common.Core.Model.NoSQL;

namespace Common.Core.Interface.NoSQL;

/// <summary>
/// Firebase Realtime database context
/// </summary>
public interface IFirebaseContext
{
    Task CreateAsync(Notification entity, int userId);
    Task CreateAsync(Notification entity, int userId, string transactionId);
}
