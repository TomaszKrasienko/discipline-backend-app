using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.Enums;
using Shouldly;
using Xunit;

namespace discipline.centre.domain.unit_tests.Users;

public sealed class PaidSubscriptionOrderTests
{
        
    [Fact]
    public void Create_GivenAllValidArgumentsForYearlySubscription_ShouldReturnSubscriptionOrderWithFilledNext()
    {
        //arrange
        var subscription = Subscription.Create(SubscriptionId.New(), "test_subscription_title", 10, 100,
            ["test"]);
        var id = SubscriptionOrderId.New();
        var subscriptionOrderFrequency = SubscriptionOrderFrequency.Yearly;
        var now = new DateTime(2024, 7, 22, 12, 30, 00, DateTimeKind.Utc);
        var paymentToken = "test_payment_token";

        //act
        var result = PaidSubscriptionOrder.Create(id, subscription, subscriptionOrderFrequency, now, paymentToken);

        //arrange
        result.Id.ShouldBe(id);
        result.CreatedAt.Value.ShouldBe(now);
        result.State.ActiveTill.ShouldBe(new DateOnly(2025, 7, 21));
        result.State.IsCancelled.ShouldBeFalse();
        result.Next.Value.ShouldBe(new DateOnly(2025, 7, 22));
        result.PaymentDetails.Token.ShouldBe(paymentToken);
        result.Type.Value.ShouldBe(subscriptionOrderFrequency);
    }
    
    [Fact]
    public void Create_GivenAllValidArgumentsForMonthlySubscription_ShouldReturnSubscriptionOrderWithFilledNext()
    {
        //arrange
        var subscription = Subscription.Create(SubscriptionId.New(), "test_subscription_title", 10, 100,
            ["test"]);
        var id = SubscriptionOrderId.New();
        var subscriptionOrderFrequency = SubscriptionOrderFrequency.Monthly;
        var now = new DateTime(2024, 7, 22, 12, 30, 00, DateTimeKind.Utc);
        var paymentToken = "payment_token";

        //act
        var result = PaidSubscriptionOrder.Create(id, subscription, subscriptionOrderFrequency, now, paymentToken);
 
        //arrange
        result.Id.ShouldBe(id);
        result.CreatedAt.Value.ShouldBe(now);
        result.State.ActiveTill.ShouldBe(new DateOnly(2024, 8, 21));
        result.State.IsCancelled.ShouldBeFalse();
        result.Next.Value.ShouldBe(new DateOnly(2024, 8, 22));
        result.PaymentDetails.Token.ShouldBe(paymentToken);
        result.Type.Value.ShouldBe(subscriptionOrderFrequency);
    }
    
    [Fact]
    public void Create_GivenFreeSubscription_ShouldThrowDomainExceptionWithCode()
    {
        //arrange
        var subscription = Subscription.Create(SubscriptionId.New(), "test_subscription_title", 0, 0,
            ["test"]);
        
        //act
        var exception = Record.Exception(() => PaidSubscriptionOrder.Create(SubscriptionOrderId.New(), subscription,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, "payment_token"));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("User.PaidSubscriptionOrder.InvalidType");
    }

    [Theory]
    [MemberData(nameof(GetInvalidPaidSubscriptionData))]
    public void Create_GivenInvalidArgument_ShouldThrowDomainException(PaidSubscriptionCreateParameters parameters)
    {
        //arrange
        var subscription = Subscription.Create(SubscriptionId.New(), "title", 10, 100,
            ["test"]);
        
        //act
        var exception = Record.Exception(() => PaidSubscriptionOrder.Create(parameters.Id, subscription, 
            parameters.SubscriptionOrderFrequency, parameters.Now, parameters.PaymentToken));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
    }

    public static IEnumerable<object[]> GetInvalidPaidSubscriptionData()
    {
        yield return [new PaidSubscriptionCreateParameters(SubscriptionOrderId.New(), SubscriptionOrderFrequency.Monthly, default, "payment_token")];
        yield return [new PaidSubscriptionCreateParameters(SubscriptionOrderId.New(), SubscriptionOrderFrequency.Monthly, DateTimeOffset.Now, string.Empty)];
    }
    
    public sealed record PaidSubscriptionCreateParameters(SubscriptionOrderId Id, SubscriptionOrderFrequency SubscriptionOrderFrequency, DateTimeOffset Now,
        string PaymentToken);
}