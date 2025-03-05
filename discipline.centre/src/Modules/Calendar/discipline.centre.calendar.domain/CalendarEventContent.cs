using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.calendar.domain;

public sealed class CalendarEventContent : ValueObject
{
    public string Title { get; }
    public string? Content { get; set; }

    private CalendarEventContent(string title, string? content)
    {
        Title = title;
        Content = content;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Title;
        yield return Content;
    }
}