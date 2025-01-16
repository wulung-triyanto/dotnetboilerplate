using Common.Core.Abstract.Scheduler;

namespace Application.Core.Scheduler;

public class SampleHandler(ISchedulerFacade facade) : AbstractHandler(facade), IRequestHandler<SampleRequest, Response<SampleResponse>>
{
    private readonly ISchedulerFacade facade = facade;

    public async Task<Response<SampleResponse>> Handle(SampleRequest request, CancellationToken cancellationToken)
    {
        var response = new Response<SampleResponse>();

        return response;
    }
}
