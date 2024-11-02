using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.users.domain.Subscriptions.ValueObjects;
using Shouldly;
using Xunit;

namespace discipline.centre.domain.unit_tests.Subscriptions.ValueObjects;

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