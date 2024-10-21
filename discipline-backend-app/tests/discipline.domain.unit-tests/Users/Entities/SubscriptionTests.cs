using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Users.Entities;

public sealed class SubscriptionTests
{
    [Fact]
    public void IsFreeSubscription_GivenSubscriptionWithZeroPrice_ShouldBeTrue()
    {
        //arrange
        var subscription = Subscription.Create(SubscriptionId.New(), "test_subscription_title", 0, 0,
            ["test"]);
        
        //act
        var result = subscription.IsFreeSubscription();
        
        //assert
        result.ShouldBeTrue();
    }
    
    [Theory]
    [InlineData(10, 0)]
    [InlineData(0, 100)]
    [InlineData(10, 100)]
    public void IsFreeSubscription_GivenSubscriptionNotWithZeroPrice_ShouldBeFalse(decimal perMonth, decimal perYear)
    {
        //arrange
        var subscription = Subscription.Create(SubscriptionId.New(), "test_subscription_title", perMonth, perYear,
            ["test"]);
        
        //act
        var result = subscription.IsFreeSubscription();
        
        //assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void AddFeature_GivenNotEmptyString_ShouldAddToFeatures()
    {
        //arrange
        var feature = "test_added_feature";
        var subscription = Subscription.Create(SubscriptionId.New(), "test_subscription_title", 1, 1,
            ["test_feature"]);
        
        //act
        subscription.AddFeature(feature);
        
        //assert
        subscription.Features.Any(x => x.Value == feature).ShouldBeTrue();
    }
    
    [Fact]
    public void AddFeature_GivenEmptyString_ShouldThrowEmptyFeatureValueException()
    {
        //arrange
        var feature = string.Empty;
        var subscription = Subscription.Create(SubscriptionId.New(), "test_subscription_title", 1, 1,
            ["test_feature"]);
        
        //act
        var exception = Record.Exception(() => subscription.AddFeature(feature));
        
        //assert
        exception.ShouldBeOfType<EmptyFeatureValueException>();
    }
}