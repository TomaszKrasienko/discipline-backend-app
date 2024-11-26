using discipline.centre.users.infrastructure.DAL.Users.Documents;

// ReSharper disable once CheckNamespace
namespace discipline.centre.users.domain.Users;

internal static class UserEntitiesMappingExtensions
{
    internal static UserDocument MapAsDocument(this User entity, string securedPassword)
        => new ()
        {
            Id = entity.Id.Value.ToString(),
            Email = entity.Email,
            Password = securedPassword,
            FirstName = entity.FullName.FirstName,
            LastName = entity.FullName.LastName,
            Status = entity.Status,
            SubscriptionOrder = entity.SubscriptionOrder?.MapAsDocument()
        };

    private static SubscriptionOrderDocument MapAsDocument(this SubscriptionOrder entity) => entity switch
    {
        FreeSubscriptionOrder freeSubscriptionOrder => freeSubscriptionOrder.MapAsDocument(),
        PaidSubscriptionOrder paidSubscriptionOrder => paidSubscriptionOrder.MapAsDocument(),
        _ => throw new ArgumentOutOfRangeException(nameof(entity), entity, null)
    };

    private static PaidSubscriptionOrderDocument MapAsDocument(this PaidSubscriptionOrder entity)
        => new ()
        {
            Id = entity.Id.Value.ToString(),
            CreatedAt = entity.CreatedAt,
            SubscriptionId = entity.SubscriptionId.Value,
            StateIsCancelled = entity.State.IsCancelled,
            StateActiveTill = entity.State.ActiveTill,
            Next = entity.Next,
            PaymentToken = entity.PaymentDetails.Token,
            Type = (int)entity.Type.Value
        };

    private static FreeSubscriptionOrderDocument MapAsDocument(this FreeSubscriptionOrder entity)
        => new ()
        {
            Id = entity.Id.Value.ToString(),
            CreatedAt = entity.CreatedAt,
            SubscriptionId = entity.SubscriptionId.Value,
            StateIsCancelled = entity.State.IsCancelled,
            StateActiveTill = entity.State.ActiveTill
        };
}