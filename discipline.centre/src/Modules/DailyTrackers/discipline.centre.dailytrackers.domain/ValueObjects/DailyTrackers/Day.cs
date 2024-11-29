using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.dailytrackers.domain.ValueObjects.DailyTrackers;

public sealed class Day : ValueObject
{
    public DateOnly Value { get; }

    private Day(DateOnly value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}