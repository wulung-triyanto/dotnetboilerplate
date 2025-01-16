namespace Common.Core.Interface.Message;

public interface IMessageContext
{
    string Message { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="topicName">Service bus topic name</param>
    /// <param name="filter">Service bus filter name</param>
    /// <param name="messages">Service bus message body</param>
    /// <returns></returns>
    Task SendMessageAsync(string topicName, string filter, List<string> messages);

    /// <summary>
    /// Send message to Azure Service Bus
    /// </summary>
    /// <param name="topicName">Azure Service Bus topic name (e.g. sbt-order)</param>
    /// <param name="filterKey">Azure Service Bus subscription filter key name (e.g. 'Label')</param>
    /// <param name="filterValue">Azure Service Bus subscription filter key value (e.g. 'mssql-persistor')</param>
    /// <param name="messages">Azure Service Bus message JSON payload</param>
    /// <returns>Task</returns>
    Task SendMessageAsync(string topicName, string filterKey, string filterValue,
                          string message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Batch send message to Azure Service Bus
    /// </summary>
    /// <param name="topicName">Azure Service Bus topic name (e.g. sbt-order)</param>
    /// <param name="filterKey">Azure Service Bus subscription filter key name (e.g. 'Label')</param>
    /// <param name="filterValue">Azure Service Bus subscription filter key value (e.g. 'mssql-persistor')</param>
    /// <param name="messages">Azure Service Bus message JSON payload</param>
    /// <returns>Task</returns>
    Task BatchSendMessageAsync(string topicName, string filterKey, string filterValue,
                               List<string> messages, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="topicName">Service bus topic name</param>
    /// <param name="subscriptionName">Service bus subscription name</param>
    /// <returns></returns>
    Task ReadMessageAsync(string topicName, string subscriptionName);
}
