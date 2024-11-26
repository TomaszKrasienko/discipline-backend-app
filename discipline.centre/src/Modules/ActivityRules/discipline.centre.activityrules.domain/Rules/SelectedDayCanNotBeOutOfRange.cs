using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules;

internal sealed class SelectedDayCanNotBeOutOfRangeRule(int value) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRule.SelectedDay.OutOfRange",
        $"Selected day: {value} is out of range for selected day");

    public bool IsBroken()
        => value is < (int)DayOfWeek.Sunday or > (int)DayOfWeek.Saturday;
}