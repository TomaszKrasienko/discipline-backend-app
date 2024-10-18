namespace discipline.domain.SharedKernel.TypeIdentifiers;

public sealed record ActivityId(Ulid Value) : ITypeId<ActivityId>
{
    public static ActivityId New()
        => new(Ulid.NewUlid());
}