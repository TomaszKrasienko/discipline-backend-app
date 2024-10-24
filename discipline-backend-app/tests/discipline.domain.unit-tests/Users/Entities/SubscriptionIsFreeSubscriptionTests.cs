using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Users.Entities;

public sealed class SubscriptionIsFreeSubscriptionTests
{
    [Theory]
    [InlineData(0,1)]
    [InlineData(1,0)]
    [InlineData(1,1)]
    public void IsFree_ShouldReturnFalse_WhenSubscriptionPricePerMonthOrPerYearIsNotZero(decimal perMonth, decimal perYear)
    {
        //arrange
        var subscription = Subscription.Create(SubscriptionId.New(), "test_title",
            perMonth, perYear, ["test"]);
        
        //act
        var result = subscription.IsFree();
        
        //assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void IsFree_ShouldReturnTrue_WhenSubscriptionPricesEqualZero()
    {
        //arrange
        var subscription = Subscription.Create(SubscriptionId.New(), "test_title",
            0, 0, ["test"]);
        
        //act
        var result = subscription.IsFree();
        
        //assert
        result.ShouldBeTrue();   
    }
}