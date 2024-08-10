using discipline.domain.ActivityRules.Exceptions;
using discipline.domain.UsersCalendars.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.UsersCalendars.Entities.Create;

public sealed class MeetingCreateTests
{
    [Theory, MemberData(nameof(GetPositivePathDate))]
    public void Create_GivenValidArguments_ShouldReturnMeeting(TimeOnly timeFrom, TimeOnly? timeTo, string platform, string uri, string place)
    {
        //arrange
        var id = Guid.NewGuid();
        var title = "test_title";
        
        //act
        var result = Meeting.Create(id, title, timeFrom, timeTo, platform, uri, place);
        
        //assert
        result.Id.Value.ShouldBe(id);
        result.Title.Value.ShouldBe(title);
        result.MeetingTimeSpan.From.ShouldBe(timeFrom);
        result.MeetingTimeSpan.To.ShouldBe(timeTo);
        result.Address.Platform.ShouldBe(platform);
        result.Address.Uri.ShouldBe(uri);
        result.Address.Place.ShouldBe(place);
    }

    public static IEnumerable<object[]> GetPositivePathDate()
        => new List<object[]>
        {
            new object[] { new TimeOnly(15, 00), new TimeOnly(16, 00), "test_platform", "test_uri", string.Empty },
            new object[] { new TimeOnly(15, 00), new TimeOnly(16, 00), "test_platform", string.Empty, string.Empty },
            new object[] { new TimeOnly(15, 00), new TimeOnly(16, 00), string.Empty, "test_uri", string.Empty },
            new object[] { new TimeOnly(15, 00), new TimeOnly(16, 00), string.Empty, string.Empty, "test_place" },
            new object[] { new TimeOnly(15, 00), null!, string.Empty, string.Empty, "test_place" },
        };
    
    [Fact]
    public void Create_GivenEmptyTitle_ShouldThrowEmptyEventTitleException()
    {
        //act
        var exception = Record.Exception(() => Meeting.Create(Guid.NewGuid(),
            string.Empty, new TimeOnly(15, 00),
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
            title, new TimeOnly(15, 00),
            null, "test", "test", null));
        
        //arrange
        exception.ShouldBeOfType<InvalidActivityRuleTitleLengthException>();
    }

    [Fact]
    public void Create_GivenTimeFromAfterTimeTo_ShouldThrowInvalidMeetingTimeSpanException()
    {
        //act
        var exception = Record.Exception(() => Meeting.Create(Guid.NewGuid(),
            "title", new TimeOnly(15, 00),
            new TimeOnly(14, 00), "test", "test", null));
        
        //arrange
        exception.ShouldBeOfType<InvalidMeetingTimeSpanException>();
    }

    [Fact]
    public void Create_GivenAllEmptyOrNullAddressFields_ShouldReturnEmptyAddressException()
    {
        //act
        var exception = Record.Exception(() => Meeting.Create(Guid.NewGuid(),
            "title", new TimeOnly(15, 00),
            new TimeOnly(16, 00),string.Empty, string.Empty, string.Empty));
        
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
            "title",  new TimeOnly(15, 00),
            new TimeOnly(16, 00), platform, uri, place));
        
        //arrange
        exception.ShouldBeOfType<InconsistentAddressTypeException>();
    }
}