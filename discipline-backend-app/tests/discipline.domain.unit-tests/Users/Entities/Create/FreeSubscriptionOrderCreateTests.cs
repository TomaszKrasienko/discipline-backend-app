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
        var subscription = Subscription.Create(Guid.NewGuid(), "test_title_subscription",
            0, 0, ["test"]);
        var id = Guid.NewGuid();
        var now = DateTime.Now;
        
        //act
        var result = FreeSubscriptionOrder.Create(id, subscription, now);
        
        //assert
        result.Id.Value.ShouldBe(id);
        result.CreatedAt.Value.ShouldBe(now);
        result.SubscriptionId.Value.ShouldBe(subscription.Id.Value);
        result.State.IsCancelled.ShouldBeFalse();
        result.State.ActiveTill.ShouldBeNull();
    }
    
    [Fact]
    public void Create_GivenNullSubscription_ShouldThrowNullSubscriptionException()
    {
        //act
        var exception = Record.Exception(() => FreeSubscriptionOrder.Create(Guid.NewGuid(), null,
            DateTime.Now));
           
        //arrange
        exception.ShouldBeOfType<NullSubscriptionException>();
    }

    [Fact]
    public void Create_GivenNotFreeSubscription_ShouldThrowInvalidSubscriptionTypeException()
    {
        //arrange
        var subscription = Subscription.Create(Guid.NewGuid(), "test_title_subscription",
            10, 100, ["test"]);
        
        //act
        var exception = Record.Exception(() => FreeSubscriptionOrder.Create(Guid.NewGuid(), subscription,
            DateTime.Now));
        
        //assert
        exception.ShouldBeOfType<InvalidSubscriptionTypeException>();
    }
}