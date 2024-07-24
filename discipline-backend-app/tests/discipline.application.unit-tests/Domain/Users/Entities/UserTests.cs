using discipline.application.Domain.Users.Enums;
using discipline.application.Domain.Users.Exceptions;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Users.Entities;

public sealed class UserTests
{
    [Fact]
    public void AddSubscriptionOrder_GivenUserWithoutSubscriptionOrder_ShouldSetSubscriptionOrder()
    {
        //arrange
        var user = UserFactory.Get();
        var subscription = SubscriptionFactory.Get();
        var subscriptionOrderId = Guid.NewGuid();
        
        //act
        user.CreateSubscriptionOrder(subscriptionOrderId, subscription,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, new string('1', 15), "123");
        
        //assert
        user.SubscriptionOrder.ShouldNotBeNull();
        user.SubscriptionOrder.Id.Value.ShouldBe(subscriptionOrderId);
    }
    
    [Fact]
    public void AddSubscriptionOrder_GivenUserWithSubscriptionOrder_ShouldThrowSubscriptionOrderForUserAlreadyExistsException()
    {
        //arrange
        var user = UserFactory.Get();
        var subscription = SubscriptionFactory.Get();
        user.CreateSubscriptionOrder(Guid.NewGuid(), subscription,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, new string('1', 15), "123");
        
        //act
        var exception = Record.Exception(() => user.CreateSubscriptionOrder(Guid.NewGuid(), subscription,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, new string('1', 15), "123"));
        
        //assert
        exception.ShouldBeOfType<SubscriptionOrderForUserAlreadyExistsException>();
    }
}