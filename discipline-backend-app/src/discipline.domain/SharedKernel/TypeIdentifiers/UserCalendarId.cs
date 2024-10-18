namespace discipline.domain.SharedKernel.TypeIdentifiers;

public sealed record UserCalendarId(Ulid Value) : ITypeId<UserCalendarId>
{
    public static UserCalendarId New()
        => new (Ulid.NewUlid());
}