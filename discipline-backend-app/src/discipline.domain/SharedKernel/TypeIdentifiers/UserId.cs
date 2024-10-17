namespace discipline.domain.SharedKernel.TypeIdentifiers;

public sealed record UserId(Ulid Value) : ITypeId
{
    public static ITypeId New()
        => new UserId(Ulid.NewUlid());

    public override string ToString()
        => Value.ToString();
}