﻿namespace Application.Core.Scheduler;

public class SampleValidator : AbstractValidator<SampleRequest>
{
    public SampleValidator()
    {
        RuleFor(x => x.userId).NotEmpty().GreaterThan(0);
    }
}
