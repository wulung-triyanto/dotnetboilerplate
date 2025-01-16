namespace Common.Core.Wrapper;

public interface IResultStatus
{
    bool IsSuccess { get; }

    object Id { get; }
    string Message { get; set; }
    string ExternalMessage { get; set; }

    IResultStatus ReturnErrorStatus(string message);
    IResultStatus ReturnErrorStatus(string message, string externalMessage);

    IResultStatus ReturnSuccessStatus();
    IResultStatus ReturnSuccessStatus(string message);
    IResultStatus ReturnSuccessStatus(string message, string externalMessage);

    void SetErrorStatus();
    void SetErrorStatus(string message);
    void SetErrorStatus(Exception ex, string message);
    void SetErrorStatus(string message, string externalMessage);

    void SetSuccessStatus();
    void SetSuccessStatus(string message);
    void SetSuccessStatus(string message, object id);
    void SetSuccessStatus(string message, string externalMessage);
}

[ExcludeFromCodeCoverage]
public class ResultStatus : IResultStatus
{
    #region Variables
    private bool status;
    private object id;
    private string message;
    private string externalMessage = string.Empty;
    #endregion

    #region Properties
    public object Id
    {
        get { return id; }
    }

    public bool IsSuccess
    {
        get { return status; }
    }
    public string Message
    {
        get { return message; }
        set { message = value; }
    }
    public string ExternalMessage
    {
        get { return externalMessage; }
        set { externalMessage = value; }
    }
    #endregion

    #region Methods
    public void SetErrorStatus()
    {
        status = false;
    }
    public void SetErrorStatus(Exception ex, string message)
    {
        throw new NotImplementedException();
    }
    public void SetErrorStatus(string message)
    {
        status = false;
        this.message = message;
    }
    public void SetErrorStatus(string message, string externalMessage)
    {
        status = false;
        this.message = message;
        this.externalMessage = externalMessage;
    }

    public void SetSuccessStatus()
    {
        status = true;
    }
    public void SetSuccessStatus(string message)
    {
        status = true;
        this.message = message;
    }
    public void SetSuccessStatus(string message, object id)
    {
        this.id = id;
        status = true;
        this.message = message;
    }
    public void SetSuccessStatus(string message, string externalMessage)
    {
        status = true;
        this.message = message;
        this.externalMessage = externalMessage;
    }

    public IResultStatus ReturnErrorStatus(string message)
    {
        status = false;
        this.message = message;
        return this;
    }
    public IResultStatus ReturnErrorStatus(string message, string externalMessage)
    {
        status = false;
        this.message = message;
        this.externalMessage = externalMessage;
        return this;
    }

    public IResultStatus ReturnSuccessStatus()
    {
        status = true;
        return this;
    }
    public IResultStatus ReturnSuccessStatus(string message)
    {
        status = true;
        this.message = message;
        return this;
    }
    public IResultStatus ReturnSuccessStatus(string message, string externalMessage)
    {
        status = true;
        this.message = message;
        this.externalMessage = externalMessage;
        return this;
    }
    #endregion
}
