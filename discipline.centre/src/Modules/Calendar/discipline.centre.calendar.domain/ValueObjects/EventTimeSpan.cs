using discipline.centre.shared.abstractions.SharedKernel;
using discpline.centre.calendar.domain.Rules.TimeEvents;

namespace discpline.centre.calendar.domain.ValueObjects;

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

    internal static EventTimeSpan Create(TimeOnly from, TimeOnly? to)
        => new (from, to);

    protected override IEnumerable<object?> GetAtomicValues()
    { 
        yield return From;
        yield return To;
    }
}