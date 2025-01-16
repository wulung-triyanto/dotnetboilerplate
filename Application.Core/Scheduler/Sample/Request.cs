namespace Application.Core.Scheduler;

[ExcludeFromCodeCoverage]
public class SampleRequest : IRequest<Response<SampleResponse>>
{
    public int userId { get; set; }
}
