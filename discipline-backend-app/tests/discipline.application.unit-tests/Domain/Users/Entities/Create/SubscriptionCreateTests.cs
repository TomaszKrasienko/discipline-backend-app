using System.Xml.XPath;
using discipline.application.Domain.Users.Entities;
using discipline.application.Domain.Users.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Users.Entities.Create;

public sealed class SubscriptionCreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnSubscriptionWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var title = "test_title";
        var pricePerMonth = 10m;
        var pricePerYear = 100m;
        
        //act
        var result = Subscription.Create(id, title, pricePerMonth, pricePerYear);
        
        //assert
        result.Id.Value.ShouldBe(id);
        result.Title.Value.ShouldBe(title);
        result.Price.PerMonth.ShouldBe(pricePerMonth);
        result.Price.PerYear.ShouldBe(pricePerYear);
    }
    
    [Fact]
    public void Create_GivenEmptyTitle_ShouldThrowEmptySubscriptionTitleException()
    {
        //act
        var exception = Record.Exception(() => Subscription.Create(Guid.NewGuid(),
            string.Empty, 12, 123));
        
        //assert
        exception.ShouldBeOfType<EmptySubscriptionTitleException>();
    }

    [Theory]
    [InlineData(-1,1)]    
    [InlineData(1,-1)]
    public void Create_GivenInvalidPrice_ShouldThrowSubscriptionValueLessThanZeroException(decimal pricePerMonth, decimal pricePerYear)
    {
        //act
        var exception = Record.Exception(() => Subscription.Create(Guid.NewGuid(),
            "test_title", pricePerMonth, pricePerYear));
        
        //assert
        exception.ShouldBeOfType<SubscriptionValueLessThanZeroException>();
    }
}