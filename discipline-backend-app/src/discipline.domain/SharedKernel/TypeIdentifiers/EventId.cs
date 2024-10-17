namespace discipline.domain.SharedKernel.TypeIdentifiers;

public sealed record EventId(Ulid Value) : ITypeId
{
    public static ITypeId New()
        => new EventId(Ulid.NewUlid());

    public override string ToString()
        => Value.ToString();
}