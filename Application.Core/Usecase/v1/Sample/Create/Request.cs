namespace Application.Core.Usecase.v1;

[ExcludeFromCodeCoverage]
public class SampleRequest : IRequest<Response<SampleResponse>>
{
    public int userId { get; set; }
}
