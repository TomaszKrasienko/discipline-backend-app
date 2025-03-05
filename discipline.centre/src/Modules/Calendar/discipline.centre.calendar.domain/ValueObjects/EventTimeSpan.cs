using discipline.centre.calendar.domain.Rules.TimeEvents;
using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.calendar.domain.ValueObjects;

public sealed class EventTimeSpan : ValueObject
{
    private readonly TimeSpan _from;
    private readonly TimeSpan? _to;
    
    public TimeSpan From
    {
        get => _from;
        private init
        {
            CheckRule(new EventTimeSpanValueCannotBeDefaultRule(value));
            _from = value;
        }
    }

    public TimeSpan? To
    {
        get => _from;
        private init
        {
            if (value is not null)
            {
                CheckRule(new EventTimeSpanValueCannotBeDefaultRule(value.Value));
            }
            _to = value;
        }
    }

    private EventTimeSpan(TimeSpan from, TimeSpan? to)
    {
        From = from;
        To = to;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return From;
        yield return To;
    }
}