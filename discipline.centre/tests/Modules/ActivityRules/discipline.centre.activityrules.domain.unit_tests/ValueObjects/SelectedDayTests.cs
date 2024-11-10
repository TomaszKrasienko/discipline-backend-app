using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unit_tests.ValueObjects;

public sealed class SelectedDayTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public void Create_GivenValueInRange_ShouldReturnSelectedDayWithValue(int value)
    {
        //act
        var result = SelectedDay.Create(value);
        
        //assert
        result.Value.ShouldBe(value);
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(7)]
    public void Create_GivenValueOutOfRange_ThrowDomainExceptionWithCodeActivityRuleSelectedDayOutOfRange(int value)
    {
        //act
        var exception = Record.Exception(() => SelectedDay.Create(value));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.SelectedDay.OutOfRange");
    }
}