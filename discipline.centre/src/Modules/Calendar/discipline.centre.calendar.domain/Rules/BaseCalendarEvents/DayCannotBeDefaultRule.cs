using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discpline.centre.calendar.domain.Rules.BaseCalendarEvents;

internal sealed class DayCannotBeDefaultRule(DateOnly value) : IBusinessRule
{
    public Exception Exception => new DomainException("CalendarEvent.Day.Default",
        "Daily tracker day cannot be default value");

    public bool IsBroken()
        => value == DateOnly.MinValue;
}