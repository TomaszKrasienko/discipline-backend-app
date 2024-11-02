namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record ActivityId(Ulid Value) : ITypeId<ActivityId>
{
    public static ActivityId New()
        => new(Ulid.NewUlid());

    public override string ToString()
        => Value.ToString();
}