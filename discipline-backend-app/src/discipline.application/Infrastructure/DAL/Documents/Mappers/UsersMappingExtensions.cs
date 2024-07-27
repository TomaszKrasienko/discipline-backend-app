using System.Diagnostics;
using discipline.application.Domain.Users.Entities;
using discipline.application.Domain.Users.ValueObjects;
using discipline.application.Infrastructure.DAL.Documents.Users;

namespace discipline.application.Infrastructure.DAL.Documents.Mappers;

internal static class UsersMappingExtensions
{
    internal static UserDocument AsDocument(this User entity)
        => new()
        {
            Id = entity.Id,
            Email = entity.Email,
            Password = entity.Password,
            FirstName = entity.FullName.FirstName,
            LastName = entity.FullName.LastName,
            SubscriptionOrder = entity.SubscriptionOrder.AsDocument()
        };

    private static SubscriptionOrderDocument AsDocument(this SubscriptionOrder entity) => entity switch
    {
        FreeSubscriptionOrder freeSubscriptionOrder => freeSubscriptionOrder.AsDocument(),
        PaidSubscriptionOrder paidSubscriptionOrder => paidSubscriptionOrder.AsDocument()
    };

    private static PaidSubscriptionOrderDocument AsDocument(this PaidSubscriptionOrder entity)
        => new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            SubscriptionId = entity.SubscriptionId,
            StateIsCancelled = entity.State.IsCancelled,
            StateActiveTill = entity.State.ActiveTill,
            Next = entity.Next,
            PaymentDetailsCardNumber = entity.PaymentDetails.CardNumber,
            PaymentDetailsCvvCode = entity.PaymentDetails.CvvCode
        };
    
    private static FreeSubscriptionOrderDocument AsDocument(this FreeSubscriptionOrder entity)
        => new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            SubscriptionId = entity.SubscriptionId,
            StateIsCancelled = entity.State.IsCancelled,
            StateActiveTill = entity.State.ActiveTill
        };

    internal static User AsEntity(this UserDocument document)
        => new(document.Id, document.Email, document.Password, new FullName(document.FirstName, document.LastName));

    
    
    internal static Subscription AsEntity(this SubscriptionDocument document)
        => new (document.Id, document.Title, new Price(document.PricePerMonth, document.PricePerYear));
}