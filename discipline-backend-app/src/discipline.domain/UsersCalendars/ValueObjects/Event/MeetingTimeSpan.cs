using discipline.domain.UsersCalendars.Policies;

namespace discipline.domain.UsersCalendars.ValueObjects.Event;

public sealed record MeetingTimeSpan
{
    public TimeOnly From { get; }
    public TimeOnly? To { get; }

    public MeetingTimeSpan(TimeOnly from, TimeOnly? to)
    {
        if (to is not null)
        {
            var policy = MeetingTimePolicy.GetInstance(from, to.Value);
            policy.Validate();
        }

        From = from;
        To = to;
    }
}