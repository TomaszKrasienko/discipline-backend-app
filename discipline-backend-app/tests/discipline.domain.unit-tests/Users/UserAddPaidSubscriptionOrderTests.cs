using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Enums;
using discipline.domain.Users.Events;
using discipline.domain.Users.Exceptions;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Users.Entities;

public sealed class UserAddPaidSubscriptionOrderTests
{
    [Fact]
    public void AddPaidSubscriptionOrder_GivenUserWithoutSubscriptionOrder_ShouldSetPaidSubscriptionOrder()
    {
        //arrange
        var user = UserFactory.Get();
        var subscription = SubscriptionFactory.Get(10, 100);
        var subscriptionOrderId = SubscriptionOrderId.New();
        
        //act
        user.CreatePaidSubscriptionOrder(subscriptionOrderId, subscription,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, new string('1', 15), "123");
        
        //assert
        user.SubscriptionOrder.ShouldNotBeNull();
        user.SubscriptionOrder.Id.ShouldBe(subscriptionOrderId);
        user.SubscriptionOrder.ShouldBeOfType<PaidSubscriptionOrder>();
        user.Status.Value.ShouldBe("PaidSubscriptionPicked");
        user.DomainEvents.Any(x => x is PaidSubscriptionPicked).ShouldBeTrue();
    }
    
    [Fact]
    public void AddPaidSubscriptionOrder_GivenUserWithPaidSubscriptionOrder_ShouldThrowSubscriptionOrderForUserAlreadyExistsException()
    {
        //arrange
        var user = UserFactory.Get();
        var subscription = SubscriptionFactory.Get(10, 100);
        user.CreatePaidSubscriptionOrder(SubscriptionOrderId.New(), subscription,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, new string('1', 15), "123");
        
        //act
        var exception = Record.Exception(() => user.CreatePaidSubscriptionOrder(SubscriptionOrderId.New(), subscription,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, new string('1', 15), "123"));
        
        //assert
        exception.ShouldBeOfType<SubscriptionOrderForUserAlreadyExistsException>();
    }
}