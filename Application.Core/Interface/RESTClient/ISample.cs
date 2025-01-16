using Common.Core.Model.Response;

namespace Application.Core.Interface.RESTClient;

public interface ISampleClient
{
    Task<Response> CreateMileageAsync(string transactionId, string request, CancellationToken cancellationToken);
}
