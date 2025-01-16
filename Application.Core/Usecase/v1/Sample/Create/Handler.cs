using Common.Core.Abstract.API;

namespace Application.Core.Usecase.v1;

public class SampleHandler(IAPIFacade facade) : AbstractHandler(facade), IRequestHandler<SampleRequest, Response<SampleResponse>>
{
    private readonly IAPIFacade facade = facade;

    public async Task<Response<SampleResponse>> Handle(SampleRequest request, CancellationToken cancellationToken)
    {
        var response = new Response<SampleResponse>();

        return response;
    }
}
