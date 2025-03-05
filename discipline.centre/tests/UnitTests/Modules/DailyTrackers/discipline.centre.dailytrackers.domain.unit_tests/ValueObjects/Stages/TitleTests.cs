using discipline.centre.dailytrackers.domain.ValueObjects.Stages;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.ValueObjects.Stages;

public sealed class TitleTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnTitleWithValue()
    {
        //arrange
        var value = "test_title";
        
        //act
        var result = Title.Create(value);
        
        //assert
        result.Value.ShouldBe(value);
    }
    
    [Fact]
    public void Create_GivenEmptyValue_ShouldThrowDomainExceptionWithCodeActivityRuleStageTitleEmpty()
    {
        //act
        var exception = Record.Exception(() => Title.Create(string.Empty));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Stage.Title.Empty");
    }

    [Fact]
    public void Create_GivenValueLongerThan30Characters_ShouldThrowDomainExceptionWithCodeActivityRuleStageTitleTooLong()
    {
        //act
        var exception = Record.Exception(() => Title.Create(new string('a', 31)));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Stage.Title.TooLong");
    }
}