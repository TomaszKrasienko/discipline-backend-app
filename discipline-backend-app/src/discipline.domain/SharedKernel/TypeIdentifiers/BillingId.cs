namespace discipline.domain.SharedKernel.TypeIdentifiers;

public sealed record BillingId(Ulid Value) : ITypeId
{
    public static ITypeId New()
        => new BillingId(Ulid.NewUlid());
}