using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Exceptions;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.domain.unit_tests.Users;

public sealed class UserAddFreeSubscriptionOderTests
{
    [Fact]
    public void AddFreeSubscriptionOder_GivenUserWithoutSubscriptionOrder_ShouldSetPaidSubscriptionOrder()
    {
        //arrange
        var user = UserFactory.Get();
        var subscription = SubscriptionFactory.Get();
        var subscriptionOrderId = SubscriptionOrderId.New();
        
        //act
        user.CreateFreeSubscriptionOrder(subscriptionOrderId, subscription, DateTime.Now);
        
        //assert
        user.SubscriptionOrder.ShouldNotBeNull();
        user.SubscriptionOrder.Id.ShouldBe(subscriptionOrderId);
        user.SubscriptionOrder.ShouldBeOfType<FreeSubscriptionOrder>();
        user.Status.Value.ShouldBe("FreeSubscriptionPicked");
    }

    [Fact]
    public void AddFreeSubscriptionOder_GivenUserWithFreeSubscription_ShouldThrowSubscriptionOrderForUserAlreadyExistsException()
    {
        //arrange
        var user = UserFactory.Get();
        var subscription = SubscriptionFactory.Get();
        user.CreateFreeSubscriptionOrder(SubscriptionOrderId.New(), subscription, DateTime.Now);
        
        //act
        var exception = Record.Exception(() => user.CreateFreeSubscriptionOrder(SubscriptionOrderId.New(), 
            subscription, DateTime.Now));
        
        //assert
        exception.ShouldBeOfType<SubscriptionOrderForUserAlreadyExistsException>();
    }
}