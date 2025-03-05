using discipline.centre.dailytrackers.domain.ValueObjects.Activities;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.ValueObjects.Activities;

public sealed class DetailsTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnDetailsWithValues()
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
    public void Create_GivenEmptyTitle_ShouldThrowDomainExceptionWithCodeDailyTrackerActivityDetailsTitleEmpty()
    {
        //act
        var exception = Record.Exception(() => Details.Create(string.Empty, null));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Activity.Details.Title.Empty");
    }

    [Fact]
    public void Create_GivenTitleLongerThan30Characters_ShouldThrowDomainExceptionWithCodeDailyTrackerActivityDetailsTitleTooLong()
    {
        //act
        var exception = Record.Exception(() => Details.Create(new string('t', 31), null));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Activity.Details.Title.TooLong");
    }
}