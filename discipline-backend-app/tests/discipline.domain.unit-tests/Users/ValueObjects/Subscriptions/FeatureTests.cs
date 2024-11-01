
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.ValueObjects.Subscriptions;
using Shouldly;
using Xunit;

namespace discipline.domain.unit_tests.Users.ValueObjects.Subscriptions;

public sealed class FeatureTests
{
    [Fact]
    public void Create_GivenValidValue_ShouldReturFeatureWithValue()
    {
        //arrange
        var value = "test_feature";
        
        //act
        var result = Feature.Create(value);
        
        //assert
        result.Value.ShouldBe(value);
    }

    [Fact]
    public void Create_GivenEmptyValue_ShouldThrowDomainExceptionWithCode()
    {
        //act
        var exception = Record.Exception(() => Feature.Create(string.Empty));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("Subscription.Feature.Empty");
    }
}