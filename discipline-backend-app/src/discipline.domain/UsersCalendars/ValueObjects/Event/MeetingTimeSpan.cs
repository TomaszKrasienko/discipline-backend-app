using discipline.domain.UsersCalendars.Policies;

namespace discipline.domain.UsersCalendars.ValueObjects.Event;

internal sealed record MeetingTimeSpan
{
    internal TimeOnly From { get; }
    internal TimeOnly? To { get; }

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