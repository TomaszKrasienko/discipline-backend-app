using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.domain.Subscriptions.ValueObjects;
using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.Enums;
using discipline.centre.users.domain.Users.ValueObjects.SubscriptionOrders;
using discipline.centre.users.domain.Users.ValueObjects.Users;
using discipline.centre.users.infrastructure.DAL.Users.Documents;

// ReSharper disable once CheckNamespace
namespace discipline.centre.users.infrastructure.DAL.Documents;

internal static class UserDocumentsMappingExtensions
{
    internal static User MapAsEntity(this UserDocument document)
        => new (
             UserId.Parse(document.Id), 
            document.Email, 
             Password.CreateHashed(document.Password), 
             FullName.Create(document.FirstName, document.LastName),
            document.Status, 
             document.SubscriptionOrder?.MapAsEntity());
    
    private static SubscriptionOrder MapAsEntity(this SubscriptionOrderDocument document) => document switch
    {
        FreeSubscriptionOrderDocument freeSubscriptionOrderDocument => freeSubscriptionOrderDocument.MapAsEntity(),
        PaidSubscriptionOrderDocument paidSubscriptionOrderDocument => paidSubscriptionOrderDocument.MapAsEntity(),
        _ => throw new ArgumentOutOfRangeException(nameof(document), document, null)
    };
    
    private static PaidSubscriptionOrder MapAsEntity(this PaidSubscriptionOrderDocument document)
        => new(
            SubscriptionOrderId.Parse(document.Id),
            new(document.SubscriptionId), 
            document.CreatedAt,
            new State(document.StateIsCancelled, document.StateActiveTill),
            new Next(document.Next),
            PaymentDetails.Create(document.PaymentToken),
            (SubscriptionOrderFrequency)document.Type);

    private static FreeSubscriptionOrder MapAsEntity(this FreeSubscriptionOrderDocument document)
        => new (
            SubscriptionOrderId.Parse(document.Id),
            document.CreatedAt, 
            new(document.SubscriptionId),
            new State(document.StateIsCancelled, document.StateActiveTill));
    
    internal static UserDto MapAsDto(this UserDocument document)
        => new UserDto()
        {
            Id = Ulid.Parse(document.Id),
            Email = document.Email,
            FirstName = document.FirstName,
            LastName = document.LastName,
            Status = document.Status,
            SubscriptionOrder = document.SubscriptionOrder?.MapAsDto()
        };
    
    private static SubscriptionOrderDto MapAsDto(this SubscriptionOrderDocument document)
        => new()
        {
            Id = Ulid.Parse(document.Id),
            CreatedAt = document.CreatedAt,
            SubscriptionId = document.SubscriptionId,
            StateIsCancelled = document.StateIsCancelled,
            StateActiveTill = document.StateActiveTill,
            Next = IsPaidSubscriptionOrder(document) ? ((PaidSubscriptionOrderDocument)document).Next : null,
            PaymentToken = IsPaidSubscriptionOrder(document) ? ((PaidSubscriptionOrderDocument)document).PaymentToken : null,
            Type = IsPaidSubscriptionOrder(document) ? ((PaidSubscriptionOrderDocument)document).Type : null
        };
    
    
    private static bool IsPaidSubscriptionOrder(SubscriptionOrderDocument document)
        => document is PaidSubscriptionOrderDocument;
}