using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.users.domain.Subscriptions.ValueObjects;
using Shouldly;
using Xunit;

namespace discipline.centre.domain.unit_tests.Subscriptions.ValueObjects;

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