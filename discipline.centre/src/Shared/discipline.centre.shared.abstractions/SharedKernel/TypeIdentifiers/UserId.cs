namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record UserId(Ulid Value) : ITypeId<UserId>
{
    public static UserId New()
        => new (Ulid.NewUlid());

    public override string ToString()
        => Value.ToString();

    public bool IsEmpty()
        => Value.Equals(Ulid.Empty);
}