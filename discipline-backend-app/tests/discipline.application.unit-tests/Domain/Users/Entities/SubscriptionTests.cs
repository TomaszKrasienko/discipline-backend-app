using discipline.application.Domain.Users.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Users.Entities;

public sealed class SubscriptionTests
{
    [Fact]
    public void IsFreeSubscription_GivenSubscriptionWithZeroPrice_ShouldBeTrue()
    {
        //arrange
        var subscription = Subscription.Create(Guid.NewGuid(), "test_subscription_title", 0, 0);
        
        //act
        var result = subscription.IsFreeSubscription();
        
        //assert
        result.ShouldBeTrue();
    }
    
    [Theory]
    [InlineData(10, 0)]
    [InlineData(0, 100)]
    [InlineData(10, 100)]
    public void IsFreeSubscription_GivenSubscriptionNotWithZeroPrice_ShouldBeFalse(decimal perMonth, decimal perYear)
    {
        //arrange
        var subscription = Subscription.Create(Guid.NewGuid(), "test_subscription_title", perMonth, perYear);
        
        //act
        var result = subscription.IsFreeSubscription();
        
        //assert
        result.ShouldBeFalse();
    }
}