using Common.Core.Abstract.API;
using Common.Core.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Common.Core.Model.NoSQL;

[ExcludeFromCodeCoverage]
public class EventHistory
{
    #region CTOR
    private readonly AbstractHandler handler = null;

    public EventHistory() { }

    public EventHistory(AbstractHandler handler) { this.handler = handler; }
    #endregion

    [BsonId]
    public string _id { get; set; } = ObjectId.GenerateNewId().ToString();
    public DateTime date { get; set; } = DateTime.UtcNow;
    public string datePartition { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd");
    public string entity { get; set; }

    private string _method;
    public string method
    {
        get { return _method; }
        set
        {
            _method = value;
            if (handler != null) { _method = handler.Method; }
        }
    }

    public string payload { get; set; }
    public string? rollbackQuery { get; set; } = null;

    private string _source;
    public string source
    {
        get { return _source; }
        set
        {
            _source = value;
            if (handler != null) { _source = handler.SourceURL; }
        }
    }

    /// <summary>
    /// By default, the data will have the status [IN PROGRESS/1] upon creation.
    /// <b>When instantiating this class from <u><i>anything other than an API handler</i></u>, please ensure the [status] value is set manually.</b>
    /// </summary>
    public int status { get; set; } = (int)EventStatus.INPROGRESS;

    private string _transactionId;
    public string transactionId
    {
        get { return _transactionId; }
        set
        {
            _transactionId = value;
            if (handler != null) { _transactionId = handler.TransactionId; }
        }
    }

    public string? transactionName { get; set; }

    private int _userId;
    public int userId
    {
        get { return _userId; }
        set
        {
            _userId = value;
            if (handler != null) { _userId = handler.UserId; }
        }
    }
}