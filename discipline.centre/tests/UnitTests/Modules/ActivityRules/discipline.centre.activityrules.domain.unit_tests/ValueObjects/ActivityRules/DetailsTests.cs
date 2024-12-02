using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unit_tests.ValueObjects.ActivityRules;

public sealed class DetailsTests
{
    [Fact]
    public void Create_GivenValidParameters_ShouldReturnDetailsWithValues()
    {
        //arrange
        var title = "test_title";
        var note = "test_note";
        
        //act
        var result = Details.Create(title, note);
        
        //assert
        result.Title.ShouldBe(title);
        result.Note.ShouldBe(note);
    }

    [Fact]
    public void Create_GivenEmptyTitle_ShouldReturnDomainExceptionWithCodeActivityRuleDetailsTitleEmpty()
    {
        //act
        var exception = Record.Exception(() => Details.Create(string.Empty, null));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Details.Title.Empty");
    }
    
    [Fact]
    public void Create_GivenTitleLongerThan30_ShouldReturnDomainExceptionWithCodeActivityRuleDetailsTitleTooLong()
    {
        //act
        var exception = Record.Exception(() => Details.Create(new string('t', 31), null));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Details.Title.TooLong");
    }
}