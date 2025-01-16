namespace Common.Core.Model.NoSQL;

[ExcludeFromCodeCoverage]
public class Notification
{
    public string transactionId { get; set; }
    public string feURL { get; set; }
    public string payload { get; set; }
    public int status { get; set; }
    public string entity { get; set; }
    public string method { get; set; }
    public DateTime createdAt { get; set; }
    public string message { get; set; }
    public bool isRead { get; set; } = false;
}