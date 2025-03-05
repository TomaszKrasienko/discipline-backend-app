using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.Commands;
using discipline.centre.users.domain.Users.Enums;

// ReSharper disable once CheckNamespace
namespace discipline.centre.users.application.Users.DTOs.Endpoints;

public static class CreateUserSubscriptionOrderDtoMapperExtensions
{
    public static CreateUserSubscriptionOrderCommand MapAsCommand(this CreateUserSubscriptionOrderDto dto,
        UserId userId, SubscriptionOrderId subscriptionOrderId)
    {
        SubscriptionOrderFrequency? frequency = Enum.GetValues<SubscriptionOrderFrequency>()
            .SingleOrDefault(frequency => frequency.ToString() == dto.SubscriptionOrderFrequency);
        
        var subscriptionId = SubscriptionId.Parse(dto.SubscriptionId.ToString());

        return new CreateUserSubscriptionOrderCommand(userId, subscriptionOrderId, subscriptionId,
            frequency.Value, dto.PaymentToken);
    }
}