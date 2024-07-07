namespace discipline.application.Domain.ValueObjects.Events;

internal sealed record MeetingTimeSpan
{
    internal TimeOnly From { get; }
    internal TimeOnly? To { get; }

    public MeetingTimeSpan(TimeOnly from, TimeOnly? to)
    {
        
    }
}