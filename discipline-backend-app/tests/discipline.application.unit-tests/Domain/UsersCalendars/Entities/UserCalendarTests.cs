using discipline.application.Domain.UsersCalendars.Entities;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.UsersCalendars.Entities;

public sealed class UserCalendarTests
{
    [Fact]
    public void AddEvent_GivenIdAndTitle_ShouldAddImportantDateToEvents()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var id = Guid.NewGuid();
        var title = "test_title";
        
        //act
        userCalendar.AddEvent(id, title);
        
        //assert
        var @event = userCalendar.Events.FirstOrDefault(x => x.Id.Equals(id));
        @event.ShouldBeOfType<ImportantDate>();
        @event.ShouldNotBeNull();
        @event.Title.Value.ShouldBe(title);
    }
    
    [Fact]
    public void AddEvent_GivenAllArgumentsForCalendarEvent_ShouldAddCalendarEventToEvents()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var id = Guid.NewGuid();
        var title = "test_title";
        var timeFrom = new TimeOnly(15, 0, 0);
        var timeTo = new TimeOnly(17, 0, 0);
        var action = "test_action";
        
        //act
        userCalendar.AddEvent(id, title, timeFrom, timeTo, action);
        
        //assert
        var @event = userCalendar.Events.FirstOrDefault(x => x.Id.Equals(id));
        @event.ShouldBeOfType<CalendarEvent>();
        @event.ShouldNotBeNull();
        @event.Title.Value.ShouldBe(title);
        ((CalendarEvent)@event).MeetingTimeSpan.From.ShouldBe(timeFrom);
        ((CalendarEvent)@event).MeetingTimeSpan.To.ShouldBe(timeTo);
        ((CalendarEvent)@event).Action.Value.ShouldBe(action);
    }
    
    [Fact]
    public void AddEvent_GivenAllArgumentsForOnlineMeeting_ShouldAddCMeetingToEvents()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var id = Guid.NewGuid();
        var title = "test_title";
        var timeFrom = new TimeOnly(15, 0, 0);
        var timeTo = new TimeOnly(17, 0, 0);
        var platform = "test_platform";
        var uri = "test_uri";
        
        //act
        userCalendar.AddEvent(id, title, timeFrom, timeTo, platform, uri, null);
        
        //assert
        var @event = userCalendar.Events.FirstOrDefault(x => x.Id.Equals(id));
        @event.ShouldBeOfType<Meeting>();
        @event.ShouldNotBeNull();
        @event.Title.Value.ShouldBe(title);
        ((Meeting)@event).MeetingTimeSpan.From.ShouldBe(timeFrom);
        ((Meeting)@event).MeetingTimeSpan.To.ShouldBe(timeTo);
        ((Meeting)@event).Address.Platform.ShouldBe(platform);
        ((Meeting)@event).Address.Uri.ShouldBe(uri);
        ((Meeting)@event).Address.Place.ShouldBeNullOrWhiteSpace();
    }

    [Fact]
    public void AddEvent_GivenAllArgumentsForOnSiteMeeting_ShouldAddCMeetingToEvents()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var id = Guid.NewGuid();
        var title = "test_title";
        var timeFrom = new TimeOnly(15, 0, 0);
        var timeTo = new TimeOnly(17, 0, 0);
        var place = "test_platform";

        //act
        userCalendar.AddEvent(id, title, timeFrom, timeTo, null, null, place);

        //assert
        var @event = userCalendar.Events.FirstOrDefault(x => x.Id.Equals(id));
        @event.ShouldBeOfType<Meeting>();
        @event.ShouldNotBeNull();
        @event.Title.Value.ShouldBe(title);
        ((Meeting)@event).MeetingTimeSpan.From.ShouldBe(timeFrom);
        ((Meeting)@event).MeetingTimeSpan.To.ShouldBe(timeTo);
        ((Meeting)@event).Address.Platform.ShouldBeNullOrWhiteSpace();
        ((Meeting)@event).Address.Uri.ShouldBeNullOrWhiteSpace();
        ((Meeting)@event).Address.Place.ShouldBe(place);
    }
}