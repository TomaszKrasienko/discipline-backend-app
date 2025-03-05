using discipline.centre.activityrules.domain.ValueObjects.Stages;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unit_tests.ValueObjects.Stages;

public sealed class TitleTests
{
    [Fact]
    public void Create_GivenValidValue_ShouldReturnTitleWithValue()
    {
        //arrange
        var value = "test_title";
        
        //act
        var result = Title.Create(value);
        
        //assert
        result.Value.ShouldBe(value);
    }

    [Fact]
    public void Create_GivenEmptyValue_ShouldThrowDomainExceptionWithActivityRuleTitleEmpty()
    {
        //act
        var exception = Record.Exception(() => Title.Create(string.Empty));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Stage.Title.Empty");
    }

    [Fact]
    public void Create_GivenValueLongerThan30Characters_ShouldThrowDomainExceptionWithCodeActivityRuleTitleTooLong()
    {
        //act
        var exception = Record.Exception(() => Title.Create(new string('t', 31)));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Stage.Title.TooLong");
    }
}