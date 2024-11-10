using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unit_tests.ValueObjects;

public sealed class ModeTests
{
    [Fact]
    public void Create_GivenAvailableNoEmptyValue_ShouldReturnModeWithValue()
    {
        //arrange
        var value = Mode.EveryDayMode;
        
        //act
        var result = Mode.Create(value.Value);
        
        //assert
        result.Value.ShouldBe(value.Value);
    }

    [Fact]
    public void Create_GivenEmptyValue_ShouldThrowDomainExceptionWithCodeActivityRuleModeEmpty()
    {
        //act
        var exception = Record.Exception(() => Mode.Create(string.Empty));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Mode.Empty");
    }
    
    [Fact]
    public void Create_GivenUnavailableValue_ShouldThrowDomainExceptionWithCodeActivityRuleModeUnavailable()
    {
        //act
        var exception = Record.Exception(() => Mode.Create("test"));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Mode.Unavailable");
    }
}