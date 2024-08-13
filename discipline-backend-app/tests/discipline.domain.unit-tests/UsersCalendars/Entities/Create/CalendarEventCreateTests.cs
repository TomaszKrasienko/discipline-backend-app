using discipline.domain.UsersCalendars.Entities;
using discipline.domain.UsersCalendars.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.UsersCalendars.Entities.Create;

public sealed class CalendarEventCreateTests
{
    [Theory, MemberData(nameof(GetCreatePositivePathDate))]
    public void Create_GivenValidArguments_ShouldReturnCalendarEventWithFilledFields(Guid id,
        string title, TimeOnly timeFrom, TimeOnly? timeTo, string action)
    {
        //act
        var result = CalendarEvent.Create(id, title, timeFrom, timeTo, action);
        
        //assert
        result.Id.Value.ShouldBe(id);
        result.Title.Value.ShouldBe(title);
        result.MeetingTimeSpan.From.ShouldBe(timeFrom);
        result.MeetingTimeSpan.To.ShouldBe(timeTo);
        result.Action?.Value.ShouldBe(action);
    }
    
    public static IEnumerable<object[]> GetCreatePositivePathDate()
        => new List<object[]>
        {
            new object[] {Guid.NewGuid(), "title", new TimeOnly(15, 00), new TimeOnly(16, 00), null!},
            new object[] {Guid.NewGuid(), "title", new TimeOnly(15, 00), new TimeOnly(16, 00), "test_action"},
            new object[] {Guid.NewGuid(), "title", new TimeOnly(15, 00), null!, "test_action"},
        };

    [Fact]
    public void Create_GivenEmptyTitle_ShouldThrowEmptyEventTitleException()
    {
        //act
        var exception = Record.Exception(() => CalendarEvent.Create(Guid.NewGuid(), string.Empty,
            new TimeOnly(14, 00), null, null));
        
        //assert
        exception.ShouldBeOfType<EmptyEventTitleException>();
    }

    [Fact]
    public void Create_GivenTimeToAfterTimeFrom_ShouldThrowInvalidMeetingTimeSpanException()
    {
        //act
        var exception = Record.Exception(() => CalendarEvent.Create(Guid.NewGuid(), "test_title",
            new TimeOnly(14, 00, 00), new TimeOnly(13, 00, 00), null));
        
        //assert
        exception.ShouldBeOfType<InvalidMeetingTimeSpanException>();
    }
}