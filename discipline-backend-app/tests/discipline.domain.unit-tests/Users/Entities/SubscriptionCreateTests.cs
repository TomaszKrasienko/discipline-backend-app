using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Users.Entities;

public sealed class SubscriptionCreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnSubscriptionWithFilledFields()
    {
        //arrange
        var id = SubscriptionId.New();
        var title = "test_title";
        var pricePerMonth = 10m;
        var pricePerYear = 100m;
        var feature = "test_feature";
        
        //act
        var result = Subscription.Create(id, title, pricePerMonth, pricePerYear,
            [feature]);
        
        //assert
        result.Id.ShouldBe(id);
        result.Title.Value.ShouldBe(title);
        result.Price.PerMonth.ShouldBe(pricePerMonth);
        result.Price.PerYear.ShouldBe(pricePerYear);
        result.Features.Any(x => x.Value == feature).ShouldBeTrue();
    }
    
    [Fact]
    public void Create_GivenEmptyTitle_ShouldThrowEmptySubscriptionTitleException()
    {
        //act
        var exception = Record.Exception(() => Subscription.Create(SubscriptionId.New(),
            string.Empty, 12, 123, ["test"]));
        
        //assert
        exception.ShouldBeOfType<EmptySubscriptionTitleException>();
    }

    [Theory]
    [InlineData(-1,1)]    
    [InlineData(1,-1)]
    public void Create_GivenInvalidPrice_ShouldThrowSubscriptionValueLessThanZeroException(decimal pricePerMonth, decimal pricePerYear)
    {
        //act
        var exception = Record.Exception(() => Subscription.Create(SubscriptionId.New(),
            "test_title", pricePerMonth, pricePerYear, ["test"]));
        
        //assert
        exception.ShouldBeOfType<SubscriptionValueLessThanZeroException>();
    }

    [Fact]
    public void Create_GivenEmptyFeaturesList_ShouldThrowEmptyFeaturesListException()
    {
        //act
        var exception = Record.Exception(() => Subscription.Create(SubscriptionId.New(),
            "test_title", 1, 1, []));
        
        //assert
        exception.ShouldBeOfType<EmptyFeaturesListException>();
    }

    [Fact]
    public void Create_GivenEmptyFeature_ShouldThrowEmptyFeatureValueException()
    {
        //act
        var exception = Record.Exception(() => Subscription.Create(SubscriptionId.New(),
            "test_title", 1, 1, [string.Empty]));
        
        //assert
        exception.ShouldBeOfType<EmptyFeatureValueException>();
    }
}