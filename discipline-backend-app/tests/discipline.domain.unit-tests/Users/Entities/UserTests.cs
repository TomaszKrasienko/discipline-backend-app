using discipline.domain.Users.Entities;
using discipline.domain.Users.Enums;
using discipline.domain.Users.Exceptions;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Users.Entities;

public sealed class UserTests
{
    [Fact]
    public void AddSubscriptionOrder_GivenUserWithoutSubscriptionOrder_ShouldSetPaidSubscriptionOrder()
    {
        //arrange
        var user = UserFactory.Get();
        var subscription = SubscriptionFactory.Get(10, 100);
        var subscriptionOrderId = Guid.NewGuid();
        
        //act
        user.CreatePaidSubscriptionOrder(subscriptionOrderId, subscription,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, new string('1', 15), "123");
        
        //assert
        user.SubscriptionOrder.ShouldNotBeNull();
        user.SubscriptionOrder.Id.Value.ShouldBe(subscriptionOrderId);
        user.SubscriptionOrder.ShouldBeOfType<PaidSubscriptionOrder>();
        user.Status.Value.ShouldBe("PaidSubscriptionPicked");
    }
    
    [Fact]
    public void AddPaidSubscriptionOrder_GivenUserWithPaidSubscriptionOrder_ShouldThrowSubscriptionOrderForUserAlreadyExistsException()
    {
        //arrange
        var user = UserFactory.Get();
        var subscription = SubscriptionFactory.Get(10, 100);
        user.CreatePaidSubscriptionOrder(Guid.NewGuid(), subscription,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, new string('1', 15), "123");
        
        //act
        var exception = Record.Exception(() => user.CreatePaidSubscriptionOrder(Guid.NewGuid(), subscription,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, new string('1', 15), "123"));
        
        //assert
        exception.ShouldBeOfType<SubscriptionOrderForUserAlreadyExistsException>();
    }
    
    [Fact]
    public void AddFreeSubscriptionOder_GivenUserWithoutSubscriptionOrder_ShouldSetPaidSubscriptionOrder()
    {
        //arrange
        var user = UserFactory.Get();
        var subscription = SubscriptionFactory.Get();
        var subscriptionOrderId = Guid.NewGuid();
        
        //act
        user.CreateFreeSubscriptionOrder(subscriptionOrderId, subscription, DateTime.Now);
        
        //assert
        user.SubscriptionOrder.ShouldNotBeNull();
        user.SubscriptionOrder.Id.Value.ShouldBe(subscriptionOrderId);
        user.SubscriptionOrder.ShouldBeOfType<FreeSubscriptionOrder>();
        user.Status.Value.ShouldBe("FreeSubscriptionPicked");
    }

    [Fact]
    public void AddFreeSubscriptionOder_GivenUserWithFreeSubscription_ShouldThrowSubscriptionOrderForUserAlreadyExistsException()
    {
        //arrange
        var user = UserFactory.Get();
        var subscription = SubscriptionFactory.Get();
        user.CreateFreeSubscriptionOrder(Guid.NewGuid(), subscription, DateTime.Now);
        
        //act
        var exception = Record.Exception(() => user.CreateFreeSubscriptionOrder(Guid.NewGuid(),
            subscription, DateTime.Now));
        
        //assert
        exception.ShouldBeOfType<SubscriptionOrderForUserAlreadyExistsException>();
    }
}