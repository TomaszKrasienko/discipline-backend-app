namespace discipline.domain.SharedKernel.TypeIdentifiers;

public sealed record UserCalendarId(Ulid Value) : ITypeId
{
    public static ITypeId New()
        => new BillingId(Ulid.NewUlid());
}