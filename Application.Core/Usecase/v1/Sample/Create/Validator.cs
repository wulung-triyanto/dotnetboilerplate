namespace Application.Core.Usecase.v1;

public class SampleValidator : AbstractValidator<SampleRequest>
{
    public SampleValidator()
    {
        RuleFor(x => x.userId).NotEmpty().GreaterThan(0);
    }
}
