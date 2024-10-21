namespace discipline.domain.SharedKernel.TypeIdentifiers;

public sealed record DailyProductivityId(Ulid Value) : ITypeId<DailyProductivityId>
{
    public static DailyProductivityId New()
        => new (Ulid.NewUlid());

    public override string ToString()
        => Value.ToString();
}