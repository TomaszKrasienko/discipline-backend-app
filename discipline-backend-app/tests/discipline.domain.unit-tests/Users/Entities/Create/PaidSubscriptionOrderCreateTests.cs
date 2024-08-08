using discipline.application.Domain.Users.Entities;
using discipline.application.Domain.Users.Enums;
using discipline.application.Domain.Users.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Users.Entities.Create;

public sealed class PaidSubscriptionOrderCreateTests
{
    [Fact]
    public void Create_GivenAllValidArgumentsForYearlySubscription_ShouldReturnSubscriptionOrderWithFilledNext()
    {
        //arrange
        var subscription = Subscription.Create(Guid.NewGuid(), "test_subscription_title", 10, 100,
            ["test"]);
        var id = Guid.NewGuid();
        var subscriptionOrderFrequency = SubscriptionOrderFrequency.Yearly;
        var now = new DateTime(2024, 7, 22, 12, 30, 00);
        var cardNumber = new string('1', 14);
        var cvvNumber = "123";

        //act
        var result = PaidSubscriptionOrder.Create(id, subscription, subscriptionOrderFrequency, now, cardNumber, cvvNumber);

        //arrange
        result.Id.Value.ShouldBe(id);
        result.CreatedAt.Value.ShouldBe(now);
        result.State.ActiveTill.ShouldBe(new DateOnly(2025, 7, 21));
        result.State.IsCancelled.ShouldBeFalse();
        result.Next.Value.ShouldBe(new DateOnly(2025, 7, 22));
        result.PaymentDetails.CardNumber.ShouldBe(cardNumber);
        result.PaymentDetails.CvvCode.ShouldBe(cvvNumber);
        result.Type.Value.ShouldBe(subscriptionOrderFrequency);
    }
    
    [Fact]
    public void Create_GivenAllValidArgumentsForMonthlySubscription_ShouldReturnSubscriptionOrderWithFilledNext()
    {
        //arrange
        var subscription = Subscription.Create(Guid.NewGuid(), "test_subscription_title", 10, 100,
            ["test"]);
        var id = Guid.NewGuid();
        var subscriptionOrderFrequency = SubscriptionOrderFrequency.Monthly;
        var now = new DateTime(2024, 7, 22, 12, 30, 00);
        var cardNumber = new string('1', 14);
        var cvvNumber = "123";

        //act
        var result = PaidSubscriptionOrder.Create(id, subscription, subscriptionOrderFrequency, now, cardNumber, cvvNumber);

        //arrange
        result.Id.Value.ShouldBe(id);
        result.CreatedAt.Value.ShouldBe(now);
        result.State.ActiveTill.ShouldBe(new DateOnly(2024, 8, 21));
        result.State.IsCancelled.ShouldBeFalse();
        result.Next.Value.ShouldBe(new DateOnly(2024, 8, 22));
        result.PaymentDetails.CardNumber.ShouldBe(cardNumber);
        result.PaymentDetails.CvvCode.ShouldBe(cvvNumber);
        result.Type.Value.ShouldBe(subscriptionOrderFrequency);
    }

    [Fact]
    public void Create_GivenNullSubscription_ShouldThrowNullSubscriptionException()
    {
        //act
        var exception = Record.Exception(() => PaidSubscriptionOrder.Create(Guid.NewGuid(), null,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, "test_card_number", "test_cvv"));
        
        //assert
        exception.ShouldBeOfType<NullSubscriptionException>();
    }
    
    [Fact]
    public void Create_GivenFreeSubscription_ShouldThrow()
    {
        //arrange
        var subscription = Subscription.Create(Guid.NewGuid(), "test_subscription_title", 0, 0,
            ["test"]);
        
        //act
        var exception = Record.Exception(() => PaidSubscriptionOrder.Create(Guid.NewGuid(), subscription,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, "test_card_number", "test_cvv"));
        
        //assert
        exception.ShouldBeOfType<InvalidSubscriptionTypeException>();
    }

    [Fact]
    public void Create_GivenNowDateAsDefaultDateTime_ShouldThrowDefaultCreatedAtException()
    {
        //arrange
        var subscription = Subscription.Create(Guid.NewGuid(), "title", 10, 100,
            ["test"]);
        
        //act
        var exception = Record.Exception(() => PaidSubscriptionOrder.Create(Guid.NewGuid(), subscription,
            SubscriptionOrderFrequency.Monthly, default, new string('1',14), "123"));
        
        //assert
        exception.ShouldBeOfType<DefaultCreatedAtException>();
    }
    
    [Theory]
    [InlineData(12)]
    [InlineData(20)]
    public void Create_GivenInvalidCardNumberLength_ShouldThrowInvalidCardLengthException(int multiplier)
    {
        //arrange
        var subscription = Subscription.Create(Guid.NewGuid(), "title", 10, 100,
            ["test"]);
        
        //act
        var exception = Record.Exception(() => PaidSubscriptionOrder.Create(Guid.NewGuid(), subscription,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, new string('1',multiplier), "123"));
        
        //assert
        exception.ShouldBeOfType<InvalidCardLengthException>();
    }
    
    [Fact]
    public void Create_GivenInvalidCvvNumberLength_ShouldThrowInvalidCvvLengthException()
    {
        //arrange
        var subscription = Subscription.Create(Guid.NewGuid(), "title", 10, 100,
            ["test"]);
        
        //act
        var exception = Record.Exception(() => PaidSubscriptionOrder.Create(Guid.NewGuid(), subscription,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, new string('1',14), "13"));
        
        //assert
        exception.ShouldBeOfType<InvalidCvvLengthException>();
    }
}