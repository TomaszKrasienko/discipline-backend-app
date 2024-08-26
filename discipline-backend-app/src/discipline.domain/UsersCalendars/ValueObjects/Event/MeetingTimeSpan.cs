using discipline.domain.SharedKernel;
using discipline.domain.UsersCalendars.Policies;

namespace discipline.domain.UsersCalendars.ValueObjects.Event;

public sealed class MeetingTimeSpan : ValueObject
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

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return From;
        yield return To;
    }
}