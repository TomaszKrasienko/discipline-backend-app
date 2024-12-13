using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Users;
using Shouldly;
using Xunit;

namespace discipline.centre.users.domain.unit_tests.Users;

public class FreeSubscriptionOrderTests
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
    public void Create_GivenNotFreeSubscription_ShouldThrowDomainExceptionWithCode()
    {
        //arrange
        var subscription = Subscription.Create(SubscriptionId.New(), "test_title_subscription",
            10, 100, ["test"]);
        
        //act
        var exception = Record.Exception(() => FreeSubscriptionOrder.Create(SubscriptionOrderId.New(), subscription,
            DateTime.Now));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe($"User.FreeSubscriptionOrder.InvalidType");
    }
}