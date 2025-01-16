using Common.Core.Abstract.API;
using Common.Core.Constant;
using Common.Core.Enum;

namespace Common.Core.Model.Request;

/// <summary>
/// Base message broker request.
/// </summary>
/// <remarks>
/// Used for: <b>API</b>
/// </remarks>
/// <param name="handler">The abstract handler</param>
public sealed class BrokerRequest
{
    #region CTOR
    private readonly AbstractHandler handler = null;

    public BrokerRequest() { }

    public BrokerRequest(AbstractHandler handler) { this.handler = handler; }
    #endregion

    public string action { get; set; }

    private string _authorization;
    /// <summary>
    /// Bearer token authorization: By default, this code retrieves the token from the HTTP context set by the JWT Filter middleware, or you can set it manually.
    /// </summary>
    public string authorization
    {
        get { return _authorization; }
        set
        {
            _authorization = value;
            if (handler != null) { _authorization = handler.BearerToken; }
        }
    }

    public object data { get; set; }

    /// <summary>
    /// The message broker's [end date] is used to determine message expiration.
    /// By default, the value is 3 minutes from current timestamp, but you can set it manually if needed.
    /// </summary>
    /// 
    public DateTime? endDate { get; set; } = DateTime.UtcNow.AddSeconds(CommonConst.MAX_DLQ_RETRY);

    public string entity { get; set; }

    private string _feURL;
    /// <summary>
    /// The source client URL that invokes the handler.
    /// By default, this code retrieves the token from the HTTP context set by the JWT Filter middleware, or you can set it manually.
    /// </summary>
    public string feURL
    {
        get { return _feURL; }
        set
        {
            _feURL = value;
            if (handler != null) { _feURL = handler.ClientHost; }
        }
    }

    public string filter { get; set; }

    private string _fullName;
    /// <summary>
    /// The user full name.
    /// By default, this code retrieves the token from the HTTP context set by the JWT Filter middleware, or you can set it manually.
    /// </summary>
    public string fullName
    {
        get { return _fullName; }
        set
        {
            _fullName = value;
            if (handler != null) { _fullName = handler.UserName; }
        }
    }

    public bool isNameUpdated { get; set; }

    public string message { get; set; }

    private string _method;
    /// <summary>
    /// The HTTP method in the controller that invokes the handler.
    /// By default, this code retrieves the token from the HTTP context set by the JWT Filter middleware, or you can set it manually.
    /// </summary>
    public string method
    {
        get { return _method; }
        set
        {
            _method = value;
            if (handler != null) { _method = handler.Method; }
        }
    }

    private string _oid;
    /// <summary>
    /// The OID that encapsulted in the JWT.
    /// By default, this code retrieves the token from the HTTP context set by the JWT Filter middleware, or you can set it manually.
    /// </summary>
    public string oid
    {
        get { return _oid; }
        set
        {
            _oid = value;
            if (handler != null) { _oid = handler.OID; }
        }
    }

    public object payload { get; set; }

    private string _source;
    /// <summary>
    /// The source client URL that invokes the handler.
    /// By default, this code retrieves the token from the HTTP context set by the JWT Filter middleware, or you can set it manually.
    /// </summary>
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
    /// The message broker's [start date].
    /// By default, this code uses the current timestamp, but you can set it manually if needed.
    /// </summary>
    public DateTime? startDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The data status.
    /// By default, the data will have the status [IN PROGRESS/1] upon creation.
    /// <b>When instantiating this class from <u><i>anything other than an API handler</i></u>, please ensure the [status] value is set manually.</b>
    /// </summary>
    public int status { get; set; } = (int)EventStatus.INPROGRESS;

    private string _transactionId;
    /// <summary>
    /// The transaction/correlation identifier. Set by client application/system.
    /// By default, this code retrieves the token from the HTTP context set by the JWT Filter middleware, or you can set it manually.
    /// </summary>
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
    /// <summary>
    /// The user identifier.
    /// By default, this code retrieves the token from the HTTP context set by the JWT Filter middleware, or you can set it manually.
    /// </summary>
    public int userId
    {
        get { return _userId; }
        set
        {
            _userId = value;
            if (handler != null) { _userId = handler.UserId; }
        }
    }

    private int _userNRP;
    /// <summary>
    /// The user's NRP.
    /// By default, this code retrieves the token from the HTTP context set by the JWT Filter middleware, or you can set it manually.
    /// </summary>
    public int userNRP
    {
        get { return _userNRP; }
        set
        {
            _userNRP = value;
            if (handler != null) { _userNRP = handler.UserNRP; }
        }
    }

    private string _preferredName;
    /// <summary>
    /// The user's preferred name, typically their Active Directory account name.
    /// By default, this code retrieves the token from the HTTP context set by the JWT Filter middleware, or you can set it manually.
    /// </summary>
    public string preferredName
    {
        get { return _preferredName; }
        set
        {
            _preferredName = value;
            if (handler != null) { _preferredName = handler.UserPreferredName; }
        }
    }
}