using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules;

internal sealed class TitleMustBeFrom2To100LengthRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("AcitivityRule.Title.InvalidLength",
        $"Title: {value} has invalid length");

    public bool IsBroken()
        => (value.Length is < 2 or > 100);
}