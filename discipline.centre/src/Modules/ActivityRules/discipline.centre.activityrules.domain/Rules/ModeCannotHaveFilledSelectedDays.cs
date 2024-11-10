using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules;

internal sealed class ModeCannotHaveFilledSelectedDays(Mode mode, List<int>? selectedDays) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRule.Mode.RequireSelectedDays",
        $"Mode: {mode} require filled selected days");

    public bool IsBroken()
        => mode != Mode.CustomMode && selectedDays is not null;
}