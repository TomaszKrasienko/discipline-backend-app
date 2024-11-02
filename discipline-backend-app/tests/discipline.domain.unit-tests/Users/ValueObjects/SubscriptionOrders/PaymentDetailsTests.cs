using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.ValueObjects.SubscriptionOrders;
using Shouldly;
using Xunit;

namespace discipline.domain.unit_tests.Users.ValueObjects.SubscriptionOrders;

public sealed class PaymentDetailsTests
{
    [Fact]
    public void Create_GivenValidValues_ShouldReturnPaymentDetailsWithValues()
    {
        //arrange
        var cardNumber = new string('1', 14);
        var cvvCode = "123";
        
        //act
        var result = PaymentDetails.Create(cardNumber, cvvCode);
        
        //assert
        result.CardNumber.ShouldBe(cardNumber);
        result.CvvCode.ShouldBe(cvvCode);
    }

    [Theory]
    [MemberData(nameof(GetInvalidPaymentDetailsData))]
    public void Create_GivenInvalidValue_ShouldThrowDomainExceptionWithCode(string cardNumber, string cvvCode, string code)
    {
        //act
        var exception = Record.Exception(() => PaymentDetails.Create(cardNumber, cvvCode));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe(code);
    }

    public static IEnumerable<object[]> GetInvalidPaymentDetailsData()
    {
        yield return [new string('1', 11), "123", "SubscriptionOrder.PaymentDetails.CardNumber.InvalidLength"];
        yield return [new string('1', 21), "123", "SubscriptionOrder.PaymentDetails.CardNumber.InvalidLength"];
        yield return [new string('1', 14), "1234", "SubscriptionOrder.PaymentDetails.CvvCode.InvalidLength"];
    }
}