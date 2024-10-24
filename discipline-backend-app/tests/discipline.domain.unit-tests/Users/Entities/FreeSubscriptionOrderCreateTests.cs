using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Users.Entities.Create;

public sealed class FreeSubscriptionOrderCreateTests
{
    [Fact]
    public void Create_GivenAllValidArguments_ShouldReturnFreeSubscriptionWithNullActiveTillField()
    {
        //arrange
        var subscription = Subscription.Create(SubscriptionId.New(), "test_title_subscription",
            0, 0, ["test"]);
        var id = SubscriptionOrderId.New();
        var now = DateTime.Now;
        
        //act
        var result = FreeSubscriptionOrder.Create(id, subscription, now);
        
        //assert
        result.Id.ShouldBe(id);
        result.CreatedAt.Value.ShouldBe(now);
        result.SubscriptionId.Value.ShouldBe(subscription.Id.Value);
        result.State.IsCancelled.ShouldBeFalse();
        result.State.ActiveTill.ShouldBeNull();
    }

    [Fact]
    public void Create_GivenNotFreeSubscription_ShouldThrowInvalidSubscriptionTypeException()
    {
        //arrange
        var subscription = Subscription.Create(SubscriptionId.New(), "test_title_subscription",
            10, 100, ["test"]);
        
        //act
        var exception = Record.Exception(() => FreeSubscriptionOrder.Create(SubscriptionOrderId.New(), subscription,
            DateTime.Now));
        
        //assert
        exception.ShouldBeOfType<InvalidSubscriptionTypeException>();
    }
}