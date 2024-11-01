using discipline.application.DTOs;
using discipline.domain.Users;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Enums;
using discipline.domain.Users.ValueObjects;
using discipline.domain.Users.ValueObjects.SubscriptionOrders;
using discipline.domain.Users.ValueObjects.Subscriptions;
using discipline.domain.Users.ValueObjects.Users;
using discipline.infrastructure.DAL.Documents.Users;

namespace discipline.infrastructure.DAL.Documents.Mappers;

internal static class UsersMappingExtensions
{
    internal static UserDocument AsDocument(this User entity)
        => new ()
        {
            Id = entity.Id.Value.ToString(),
            Email = entity.Email,
            Password = entity.Password,
            FirstName = entity.FullName.FirstName,
            LastName = entity.FullName.LastName,
            Status = entity.Status,
            SubscriptionOrder = entity.SubscriptionOrder?.AsDocument()
        };

    private static SubscriptionOrderDocument AsDocument(this SubscriptionOrder entity) => entity switch
    {
        FreeSubscriptionOrder freeSubscriptionOrder => freeSubscriptionOrder.AsDocument(),
        PaidSubscriptionOrder paidSubscriptionOrder => paidSubscriptionOrder.AsDocument()
    };

    private static PaidSubscriptionOrderDocument AsDocument(this PaidSubscriptionOrder entity)
        => new ()
        {
            Id = entity.Id.Value.ToString(),
            CreatedAt = entity.CreatedAt,
            SubscriptionId = entity.SubscriptionId.Value,
            StateIsCancelled = entity.State.IsCancelled,
            StateActiveTill = entity.State.ActiveTill,
            Next = entity.Next,
            PaymentDetailsCardNumber = entity.PaymentDetails.CardNumber,
            PaymentDetailsCvvCode = entity.PaymentDetails.CvvCode,
            Type = (int)entity.Type.Value
        };
    
    private static FreeSubscriptionOrderDocument AsDocument(this FreeSubscriptionOrder entity)
        => new ()
        {
            Id = entity.Id.Value.ToString(),
            CreatedAt = entity.CreatedAt,
            SubscriptionId = entity.SubscriptionId.Value,
            StateIsCancelled = entity.State.IsCancelled,
            StateActiveTill = entity.State.ActiveTill
        };

    internal static User AsEntity(this UserDocument document)
        => new (new(Ulid.Parse(document.Id)), document.Email, document.Password, FullName.Create(document.FirstName, document.LastName),
            document.Status, document.SubscriptionOrder?.AsEntity());

    private static SubscriptionOrder AsEntity(this SubscriptionOrderDocument document) => document switch
    {
        FreeSubscriptionOrderDocument freeSubscriptionOrderDocument => freeSubscriptionOrderDocument.AsEntity(),
        PaidSubscriptionOrderDocument paidSubscriptionOrderDocument => paidSubscriptionOrderDocument.AsEntity()
    };
    
    private static PaidSubscriptionOrder AsEntity(this PaidSubscriptionOrderDocument document)
        => new (new(Ulid.Parse(document.Id)), new(document.SubscriptionId), document.CreatedAt,
            new State(document.StateIsCancelled, document.StateActiveTill),
            new Next(document.Next),
            PaymentDetails.Create(document.PaymentDetailsCardNumber, document.PaymentDetailsCvvCode),
            (SubscriptionOrderFrequency)document.Type);

    private static FreeSubscriptionOrder AsEntity(this FreeSubscriptionOrderDocument document)
        => new (new(Ulid.Parse(document.Id)),  document.CreatedAt, new(document.SubscriptionId),
            new State(document.StateIsCancelled, document.StateActiveTill));

    internal static UserDto AsDto(this UserDocument document)
        => new UserDto()
        {
            Id = Ulid.Parse(document.Id),
            Email = document.Email,
            FirstName = document.FirstName,
            LastName = document.LastName,
            Status = document.Status,
            SubscriptionOrder = document.SubscriptionOrder?.AsDto()
        };
    
    private static SubscriptionOrderDto AsDto(this SubscriptionOrderDocument document)
        => new()
        {
            Id = Ulid.Parse(document.Id),
            CreatedAt = document.CreatedAt,
            SubscriptionId = document.SubscriptionId,
            StateIsCancelled = document.StateIsCancelled,
            StateActiveTill = document.StateActiveTill,
            Next = IsPaidSubscriptionOrder(document) ? ((PaidSubscriptionOrderDocument)document).Next : null,
            PaymentDetailsCardNumber = IsPaidSubscriptionOrder(document) ? ((PaidSubscriptionOrderDocument)document).PaymentDetailsCardNumber : null,
            PaymentDetailsCvvCode = IsPaidSubscriptionOrder(document) ? ((PaidSubscriptionOrderDocument)document).PaymentDetailsCvvCode : null,
            Type = IsPaidSubscriptionOrder(document) ? ((PaidSubscriptionOrderDocument)document).Type : null
        };

    private static bool IsPaidSubscriptionOrder(SubscriptionOrderDocument document)
        => document.GetType() == typeof(PaidSubscriptionOrderDocument);
    
    internal static Subscription AsEntity(this SubscriptionDocument document)
        => new (new(Ulid.Parse(document.Id)), document.Title, Price.Create(document.PricePerMonth, 
            document.PricePerYear), document.Features.Select(Feature.Create).ToList());

    internal static SubscriptionDocument AsDocument(this Subscription entity)
        => new()
        {
            Id = entity.Id.ToString(),
            PricePerMonth = entity.Price.PerMonth,
            PricePerYear = entity.Price.PerYear,
            Title = entity.Title,
            IsPaid = !entity.IsFree(),
            Features = entity.Features.Select(x => x.Value).ToList()
        };

    internal static SubscriptionDto AsDto(this SubscriptionDocument document)
        => new()
        {
            Id = Ulid.Parse(document.Id),
            Title = document.Title,
            PricePerMonth = document.PricePerMonth,
            PricePerYear = document.PricePerYear,
            IsPaid = document.IsPaid,
            Features = document.Features
        };
}