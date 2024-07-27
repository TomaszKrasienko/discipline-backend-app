using discipline.application.Domain.SharedKernel;
using discipline.application.Domain.Users.ValueObjects;

namespace discipline.application.Domain.Users.Entities;

internal sealed class Billing
{
    public EntityId Id { get; }
    public CreatedAt CreatedAt { get; }
    public IsRealized IsRealized { get; private set; }
    public Cost Cost { get; private set; }
    public TransferDetails TransferDetails { get; private set; }

    private Billing(EntityId id, CreatedAt createdAt)
    {        
        Id = id;
        CreatedAt = createdAt;
    }
    
    //For mongo
    internal Billing(EntityId id, CreatedAt createdAt, IsRealized isRealized, Cost cost, TransferDetails transferDetails)
        : this(id, createdAt)
    {
        IsRealized = isRealized;
        Cost = cost;
        TransferDetails = transferDetails;
    }

    internal static Billing Create(Guid id, DateTime createdAt, bool isRealized, decimal cost,
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