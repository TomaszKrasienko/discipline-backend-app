using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.DTOs.Endpoints;
using discipline.centre.users.domain.Users.Enums;
using Shouldly;
using Xunit;

namespace discipline.centre.users.application.unit_tests.Users.Mappers;

public sealed class CreateUserSubscriptionOrderDtoMapperExtensionsTests
{
    [Fact]
    public void MapAsCommand_GivenValidCreateUserSubscriptionOrderWithUserIdAndSubscriptionOrderId_ShouldMapToCommand()
    {
        //arrange 
        var dto = new CreateUserSubscriptionOrderDto(Ulid.NewUlid(), SubscriptionOrderFrequency.Monthly.ToString(),
            Guid.NewGuid().ToString());

        var userId = UserId.New();
        var subscriptionOrderId = SubscriptionOrderId.New();
        
        //act
        var result = dto.MapAsCommand(userId, subscriptionOrderId);
        
        //assert
        result.Id.ShouldBe(subscriptionOrderId);
        result.UserId.ShouldBe(userId);
        result.SubscriptionOrderFrequency.ShouldBe(SubscriptionOrderFrequency.Monthly);
        result.SubscriptionId.Value.ShouldBe(dto.SubscriptionId);
        result.PaymentToken.ShouldBe(dto.PaymentToken);
    }
}