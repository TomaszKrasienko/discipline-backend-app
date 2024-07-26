using discipline.application.Domain.Users.Entities;
using discipline.application.Domain.Users.Enums;
using discipline.application.Domain.Users.Exceptions;
using discipline.application.Domain.Users.Services;
using discipline.application.Domain.Users.Services.Abstractions;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Users.Services;

public sealed class SubscriptionOrderServiceTests
{
    [Fact]
    public void AddOrderSubscriptionToUser_GivenFreeSubscription_ShouldAddFreeSubscriptionToUser()
    {
        //arrange
        var user = UserFactory.Get();
        var subscription = SubscriptionFactory.Get();
        var id = Guid.NewGuid();
        
        //act
        _subscriptionOrderService.AddOrderSubscriptionToUser(user, id, subscription,
            null, DateTime.Now, null, null);
        
        //assert
        user.SubscriptionOrder.Id.Value.ShouldBe(id);
        user.SubscriptionOrder.ShouldBeOfType<FreeSubscriptionOrder>();
    }

    [Fact]
    public void AddOrderSubscriptio_GivenNullSubscription_ShouldThrowNullSubscriptionException()
    {
        //arrange
        var user = UserFactory.Get();
        
        //act
        var exception = Record.Exception(() => _subscriptionOrderService.AddOrderSubscriptionToUser(user, Guid.NewGuid(), null,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, "123", "123"));
        
        //assert
        exception.ShouldBeOfType<NullSubscriptionException>();
    }
    
    [Fact]
    public void AddOrderSubscriptio_GivenNullUser_ShouldThrowNullUserException()
    {
        //arrange
        var user = UserFactory.Get();
        
        //act
        var exception = Record.Exception(() => _subscriptionOrderService.AddOrderSubscriptionToUser(user, Guid.NewGuid(), null,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, "123", "123"));
        
        //assert
        exception.ShouldBeOfType<NullUserException>();
    }

    [Fact]
    public void AddOrderSubscription_GivenPaidSubscriptionAndNullSubscriptionOrderFrequency_ShouldThrowNullSubscriptionOrderFrequencyException()
    {
        //arrange
        var user = UserFactory.Get();
        var subscription = SubscriptionFactory.Get();
        
        //act
        var exception = Record.Exception(() => _subscriptionOrderService.AddOrderSubscriptionToUser(user,
            Guid.NewGuid(), subscription, null, DateTime.Now, "123", "123"));
        
        //assert
        exception.ShouldBeOfType<NullSubscriptionOrderFrequencyException>();
    }

    #region arrange
    private readonly ISubscriptionOrderService _subscriptionOrderService;

    public SubscriptionOrderServiceTests()
        => _subscriptionOrderService = new SubscriptionOrderService();
    #endregion
}