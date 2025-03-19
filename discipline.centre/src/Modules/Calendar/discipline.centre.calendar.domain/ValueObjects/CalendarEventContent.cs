using discipline.centre.calendar.domain.Rules.BaseCalendarEvents;
using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.calendar.domain.ValueObjects;

public sealed class CalendarEventContent : ValueObject
{
    private readonly string _title = null!;
    
    public string Title
    {
        get => _title;
        private init
        {
            CheckRule(new CalendarEventTitleCannotBeEmptyRule(value));
            _title = value;
        }
    }
    public string? Description { get; }

    private CalendarEventContent(string title, string? description)
    {
        Title = title;
        Description = description;
    }
    
    public static CalendarEventContent Create(string title, string? description) 
        => new(title, description);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Title;
        yield return Description;
    }
}