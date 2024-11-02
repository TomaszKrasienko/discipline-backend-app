namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record BillingId(Ulid Value) : ITypeId<BillingId>
{
    public static BillingId New()
        => new (Ulid.NewUlid());
}