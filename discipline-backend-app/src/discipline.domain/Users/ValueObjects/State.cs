namespace discipline.domain.Users.ValueObjects;

public sealed record State
{
    public bool IsCancelled { get; set; }
    public DateOnly? ActiveTill { get; set; }

    public State(bool isCancelled, DateOnly? activeTill)
    {
        IsCancelled = isCancelled;
        ActiveTill = activeTill;
    }
}