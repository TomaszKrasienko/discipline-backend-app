using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.ValueObjects;
using discipline.domain.Users.ValueObjects.SubscriptionOrders;

namespace discipline.domain.Users.Entities;

public sealed class Billing : Entity<BillingId>
{
    public CreatedAt CreatedAt { get; }
    public IsRealized IsRealized { get; private set; }
    public Cost Cost { get; private set; }
    public TransferDetails TransferDetails { get; private set; }

    private Billing(BillingId id, CreatedAt createdAt) : base(id)
        => CreatedAt = createdAt;
    
    //For mongo
    public Billing(BillingId id, CreatedAt createdAt, IsRealized isRealized, Cost cost, 
        TransferDetails transferDetails) : this(id, createdAt)
    {
        IsRealized = isRealized;
        Cost = cost;
        TransferDetails = transferDetails;
    }

    internal static Billing Create(BillingId id, DateTime createdAt, bool isRealized, decimal cost,
        string title, string cardNumber)
    {
        var billing = new Billing(id, createdAt);
        billing.ChangeRealization(isRealized);
        billing.ChangeCost(cost);
        billing.ChangeTransferDetails(title, cardNumber);
        return billing;
    }

    private void ChangeRealization(bool value)
        => IsRealized = value;

    private void ChangeCost(decimal value)
        => Cost = value;

    private void ChangeTransferDetails(string title, string cardNumber)
        => TransferDetails = new TransferDetails(title, cardNumber);
}