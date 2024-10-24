using discipline.domain.Users.Entities;

namespace discipline.domain.SharedKernel.TypeIdentifiers;

public sealed record UserId(Ulid Value) : ITypeId<UserId>
{
    public static UserId New()
        => new (Ulid.NewUlid());

    public override string ToString()
        => Value.ToString();

    public bool IsEmpty()
        => Value == Ulid.Empty;
}