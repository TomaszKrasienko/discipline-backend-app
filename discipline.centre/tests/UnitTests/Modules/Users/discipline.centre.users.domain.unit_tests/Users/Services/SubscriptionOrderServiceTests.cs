using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.Enums;
using discipline.centre.users.domain.Users.Services;
using discipline.centre.users.tests.sharedkernel.Domain;
using Shouldly;
using Xunit;

namespace discipline.centre.domain.unit_tests.Users.Services;

public sealed class SubscriptionOrderServiceTests
{
    [Fact]
    public void AddOrderSubscriptionToUser_GivenFreeSubscription_ShouldAddFreeSubscriptionToUser()
    {
        //arrange
        var user = UserFakeDataFactory.Get();
        var subscription = SubscriptionFakeDataFactory.Get();
        var id = SubscriptionOrderId.New();
        
        //act
        _subscriptionOrderService.AddOrderSubscriptionToUser(user, id, subscription,
            null, DateTime.Now, null);
        
        //assert
        user.SubscriptionOrder!.Id.ShouldBe(id);
        user.SubscriptionOrder.ShouldBeOfType<FreeSubscriptionOrder>();
    }

    [Fact]
    public void AddOrderSubscriptionToUser_GivenPaidSubscription_ShouldAddPaidSubscriptionToUser()
    {
        //arrange
        var user = UserFakeDataFactory.Get();
        var subscription = SubscriptionFakeDataFactory.Get(10, 100);
        var id = SubscriptionOrderId.New();

        //act
        _subscriptionOrderService.AddOrderSubscriptionToUser(user, id, subscription,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, Guid.NewGuid().ToString());

        //assert
        user.SubscriptionOrder!.Id.ShouldBe(id);
        user.SubscriptionOrder.ShouldBeOfType<PaidSubscriptionOrder>();
    }

    [Fact]
    public void AddOrderSubscriptionToUser_GivenPaidSubscriptionAndNullSubscriptionOrderFrequency_ShouldThrowDomainExceptionWithCodeSubscriptionOrderFrequencyNotFound()
    {
        //arrange
        var user = UserFakeDataFactory.Get();
        var subscription = SubscriptionFakeDataFactory.Get(10, 100);
        
        //act
        var exception = Record.Exception(() => _subscriptionOrderService.AddOrderSubscriptionToUser(user,
            SubscriptionOrderId.New(), subscription, null, DateTime.Now, Guid.NewGuid().ToString()));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("SubscriptionOrder.Frequency.NotFound");
    }

    [Fact]
    public void AddOrderSubscriptionToUser_GivenPaidSubscriptionAndNullPaymentToken_ShouldThrowDomainExceptionWithCodeSubscriptionOrderPaymentTokenNull()
    {
        var user = UserFakeDataFactory.Get();
        var subscription = SubscriptionFakeDataFactory.Get(10, 100);
        
        //act
        var exception = Record.Exception(() => _subscriptionOrderService.AddOrderSubscriptionToUser(user,
            SubscriptionOrderId.New(), subscription, SubscriptionOrderFrequency.Monthly, DateTime.Now, null));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("SubscriptionOrder.PaymentToken.Null");
    }

    #region arrange
    private readonly ISubscriptionOrderService _subscriptionOrderService;

    public SubscriptionOrderServiceTests()
        => _subscriptionOrderService = new SubscriptionOrderService();
    #endregion
}