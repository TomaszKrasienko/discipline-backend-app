using discipline.application.Domain.Entities;
using discipline.application.Domain.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Entities.UserCalendar;

public sealed class MeetingCreateTests
{
    [Theory]
    [InlineData()]
    
    [Fact]
    public void Create_GivenEmptyTitle_ShouldThrowEmptyEventTitleException()
    {
        //act
        var exception = Record.Exception(() => Meeting.Create(Guid.NewGuid(),
            string.Empty, new DateOnly(2024, 1, 1), new TimeOnly(15, 00),
            null, "test", "test", null));
        
        //arrange
        exception.ShouldBeOfType<EmptyEventTitleException>();
    }
    
    [Theory]
    [InlineData('t', 1)]
    [InlineData('t',110)]
    public void Create_GivenInvalidLengthTitle_ShouldThrowInvalidActivityRuleTitleLengthException(char text, int multiplier)
    {
        //arrange
        string title = new string(text, multiplier);
        
        //act
        var exception = Record.Exception(() => Meeting.Create(Guid.NewGuid(),
            title, new DateOnly(2024, 1, 1), new TimeOnly(15, 00),
            null, "test", "test", null));
        
        //arrange
        exception.ShouldBeOfType<InvalidActivityRuleTitleLengthException>();
    }

    [Fact]
    public void Create_GivenTimeFromAfterTimeTo_ShouldThrowInvalidMeetingTimeSpanException()
    {
        //act
        var exception = Record.Exception(() => Meeting.Create(Guid.NewGuid(),
            "title", new DateOnly(2024, 1, 1), new TimeOnly(15, 00),
            new TimeOnly(14, 00), "test", "test", null));
        
        //arrange
        exception.ShouldBeOfType<InvalidActivityRuleTitleLengthException>();
    }

    [Fact]
    public void Create_GivenAllEmptyOrNullAddressFields_ShouldReturnEmptyAddressException()
    {
        //act
        var exception = Record.Exception(() => Meeting.Create(Guid.NewGuid(),
            "title", new DateOnly(2024, 1, 1), new TimeOnly(15, 00),
            new TimeOnly(14, 00),string.Empty, string.Empty, string.Empty));
        
        //arrange
        exception.ShouldBeOfType<EmptyAddressException>();   
    }

    [Theory]
    [InlineData("test_platform", null, "test_place")]
    [InlineData(null, "test_meeting_address", "test_place")]
    public void Create_GivenInconsistentAddress_ShouldThrowInconsistentAddressTypeException(string platform, string uri, string place)
    {
        //act
        var exception = Record.Exception(() => Meeting.Create(Guid.NewGuid(),
            "title", new DateOnly(2024, 1, 1), new TimeOnly(15, 00),
            new TimeOnly(14, 00), platform, uri, place));
        
        //arrange
        exception.ShouldBeOfType<InconsistentAddressTypeException>();
    }
}