using discipline.centre.calendar.domain.Rules.TimeEvents;
using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.calendar.domain.ValueObjects;

public sealed class EventTimeSpan : ValueObject
{
    private readonly TimeOnly? _to;

    public TimeOnly From { get; }
    
    public TimeOnly? To
    {
        get => _to;
        private init
        {
            CheckRule(new EventTimeFromValueCannotBeLaterThanToValueRule(From, value));
            _to = value;
        }
    }

    private EventTimeSpan(TimeOnly from, TimeOnly? to)
    {
        From = from;
        To = to;
    }

    public static EventTimeSpan Create(TimeOnly from, TimeOnly? to)
        => new (from, to);

    protected override IEnumerable<object?> GetAtomicValues()
    { 
        yield return From;
        yield return To;
    }
}