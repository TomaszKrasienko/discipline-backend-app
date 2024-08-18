using discipline.domain.SharedKernel;

namespace discipline.domain.Users.ValueObjects;

public sealed class State : ValueObject
{
    public bool IsCancelled { get; set; }
    public DateOnly? ActiveTill { get; set; }

    public State(bool isCancelled, DateOnly? activeTill)
    {
        IsCancelled = isCancelled;
        ActiveTill = activeTill;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return IsCancelled;
        yield return ActiveTill;
    }
}