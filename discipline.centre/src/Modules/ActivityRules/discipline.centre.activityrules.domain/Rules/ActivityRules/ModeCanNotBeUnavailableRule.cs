using System.Collections.Immutable;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules;

internal sealed class ModeCanNotBeUnavailableRule(string value, ImmutableList<string> availableValues) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRule.Mode.Unavailable",
        $"Mode: {value} is unavailable");

    public bool IsBroken()
        => !availableValues.Contains(value);
}