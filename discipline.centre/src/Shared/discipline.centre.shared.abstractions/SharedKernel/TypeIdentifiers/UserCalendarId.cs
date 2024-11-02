namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record UserCalendarId(Ulid Value) : ITypeId<UserCalendarId>
{
    public static UserCalendarId New()
        => new (Ulid.NewUlid());

    public override string ToString()
        => Value.ToString();
}