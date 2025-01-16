using Common.Core.Constant;
using Common.Core.Model.Response;
using RestSharp;

namespace Security.Core.RBAC;

public interface IRBACClient
{
    Task<Response<IEnumerable<RBACModel>>?> GetRBACAsync(string transactionId, int appId, string oid, string attributeId, CancellationToken cancellationToken);

    Task<Response<DRBACModel>> GetDRBACAsync(string transactionId, string userId, CancellationToken cancellationToken);
}

public class RBACClient : IRBACClient, IDisposable
{
    private readonly RestClient restClient;

    public RBACClient()
    {
        this.restClient = new RestClient(CommonConst.RBAC_URL);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing) { restClient?.Dispose(); }
    }

    ~RBACClient()
    {
        Dispose(false);
    }

    public async Task<Response<IEnumerable<RBACModel>>?> GetRBACAsync(
        string transactionId, int appId, string oid, string attributeId, CancellationToken cancellationToken)
    {
        var response = new Response<IEnumerable<RBACModel>>();

        try
        {
            var r = new RestRequest($"role-permissions/rbac/{oid}", Method.Get);
            r.AddHeader(CommonConst.HEADER_TRANSACTION_ID, transactionId);
            r.AddQueryParameter("appId", appId);
            r.AddQueryParameter("attributeId", attributeId);

            var rest = new RestClient(CommonConst.USER_URL);
            response = await rest.GetAsync<Response<IEnumerable<RBACModel>>>(r, cancellationToken: cancellationToken);

            if (response == null || !response.Data.Any())
            {
                return response;
            }

            response.Status = Common.Core.Enum.ResponseStatus.SUCCESS;

            return response;
        }
        catch
        {
            response.Status = Common.Core.Enum.ResponseStatus.FAIL;
            return response;
        }
    }

    public async Task<Response<DRBACModel>> GetDRBACAsync(
        string transactionId, string userId, CancellationToken cancellationToken)
    {
        Response<DRBACModel> response = new()
        {
            Status = Common.Core.Enum.ResponseStatus.FAIL
        };

        var r = new RestRequest("role-positions/data-rbac", Method.Get);
        r.AddHeader(CommonConst.HEADER_TRANSACTION_ID, transactionId);
        r.AddUrlSegment("userId", userId);

        response = await restClient.GetAsync<Response<DRBACModel>>(r, cancellationToken);

        if (response != null && response.Data != null)
        {
            response.Status = Common.Core.Enum.ResponseStatus.SUCCESS;
        }

        return response;
    }
}