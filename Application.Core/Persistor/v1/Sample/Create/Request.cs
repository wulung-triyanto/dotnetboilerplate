namespace Application.Core.Persistor.v1;

[ExcludeFromCodeCoverage]
public class SampleRequest : IRequest<IResultStatus>
{
    public int userId { get; set; }
}
