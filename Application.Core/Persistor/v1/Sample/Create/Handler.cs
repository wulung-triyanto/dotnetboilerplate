using Common.Core.Abstract.Worker;

namespace Application.Core.Persistor.v1;

public class SampleHandler(IPersistorFacade facade) :
             AbstractHandler(facade), IRequestHandler<SampleRequest, IResultStatus>
{
    private readonly IPersistorFacade facade = facade;

    public async Task<IResultStatus> Handle(SampleRequest request, CancellationToken cancellationToken)
    {
        IResultStatus response = new ResultStatus();

        return response;
    }
}
