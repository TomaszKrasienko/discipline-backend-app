using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.ValueObjects.Subscriptions;
using Shouldly;
using Xunit;

namespace discipline.domain.unit_tests.Users.ValueObjects.Subscriptions;

public sealed class PriceTests
{
    [Fact]
    public void Create_GivenValidPricePerYearAndPerMonth_ShouldReturnPriceWithPerYearAndPerMonthValues()
    {
        //arrange
        var pricePerYear = 234m;
        var pricePerMonth = 32m;
        
        //act
        var result = Price.Create(pricePerMonth, pricePerYear);
        
        //assert
        result.PerMonth.ShouldBe(pricePerMonth);
        result.PerYear.ShouldBe(pricePerYear);
    }

    [Theory]
    [MemberData(nameof(GetInvalidPrice))]
    public void Create_GivenInvalidPrice_ShouldThrowDomainExceptionWithCode(decimal perMonth, decimal perYear,
        string code)
    {
        //act
        var exception = Record.Exception(() => Price.Create(perMonth, perYear));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe(code);
    }

    public static IEnumerable<object[]> GetInvalidPrice()
    {
        yield return [-1m, 3m, "Subscription.Price.PerMonth.LessThanZero"];
        yield return [1m, -1m, "Subscription.Price.PerYear.LessThanZero"];
    }
}