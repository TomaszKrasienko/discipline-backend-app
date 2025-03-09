using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.calendar.domain.Rules.BaseCalendarEvents;

internal sealed class CalendarEventTitleCannotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("CalendarEvent.Content.Title.Empty",
        "Calendar event title cannot be empty.");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}