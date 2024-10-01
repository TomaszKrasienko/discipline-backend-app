using discipline.domain.UsersCalendars.Entities;
using discipline.domain.UsersCalendars.Exceptions;
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

    [Fact]
    public void AddEvent_GivenIvent_ShouldAddEvent()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var @event = ImportantDate.Create(Guid.NewGuid(), "test_title");
        
        //act
        userCalendar.AddEvent(@event);
        
        //assert
        userCalendar.Events.Any(x => x.Id == @event.Id && x.Title == @event.Title)
            .ShouldBeTrue();
    }

    [Fact]
    public void EditEvent_GivenArgumentsForImportantDate_ShouldEditImportantDate()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var id = Guid.NewGuid();
        userCalendar.AddEvent(id, "test_important_date");
        var newImportantDateTitle = "new_test_important_date";
        
        //act
        userCalendar.EditEvent(id, newImportantDateTitle);
        
        //assert
        var importantDate = userCalendar
            .Events
            .First(x => x.Id.Value == id);
        importantDate.Title.Value.ShouldBe(newImportantDateTitle);
    }

    [Fact]
    public void EditEvent_GivenNotExistingIdAndArgumentsForImportantDate_ShouldThrowEventNotFoundException()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        
        //act
        var exception = Record.Exception(() => userCalendar.EditEvent(Guid.NewGuid(), "test_title"));
        
        //assert
        exception.ShouldBeOfType<EventNotFoundException>();
    }

    [Fact]
    public void EditEvent_GivenInvalidEventTypeIdAndArgumentsForImportantDate_ShouldThrowInvalidEventTypeIdException()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var id = Guid.NewGuid();
        userCalendar.AddEvent(id, "test_calendar_event", new TimeOnly(12,00,00),
            null, "test_action");
        
        //act
        var exception = Record.Exception(() => userCalendar.EditEvent(id, "test"));
        
        //assert
        exception.ShouldBeOfType<InvalidEventTypeIdException>();
    }
    
    [Fact]
    public void EditEvent_GivenArgumentsForCalendarEvent_ShouldEditCalendarEvent()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var id = Guid.NewGuid();
        userCalendar.AddEvent(id, "test_calendar_event_title", new TimeOnly(12,00),
            null, "test_calendar_event_action");
        var newCalendarEventTitle = "new_calendar_event_title";
        var newCalendarEventTimeFrom = new TimeOnly(13, 00);
        var newCalendarEventTimeTo = new TimeOnly(14, 00);
        var newCalendarEventAction = "new_calendar_event_action";
        
        //act
        userCalendar.EditEvent(id, newCalendarEventTitle, newCalendarEventTimeFrom, newCalendarEventTimeTo,
            newCalendarEventAction);
        
        //assert
        var calendarEvent = userCalendar
            .Events
            .First(x => x.Id.Value == id);
        ((CalendarEvent)calendarEvent).Title.Value.ShouldBe(newCalendarEventTitle);
        ((CalendarEvent)calendarEvent).MeetingTimeSpan.From.ShouldBe(newCalendarEventTimeFrom);
        ((CalendarEvent)calendarEvent).MeetingTimeSpan.To.ShouldBe(newCalendarEventTimeTo);
        ((CalendarEvent)calendarEvent).Action.Value.ShouldBe(newCalendarEventAction);
    }

    [Fact]
    public void EditEvent_GivenNotExistingIdAndArgumentsForCalendarEvent_ShouldThrowEventNotFoundException()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        
        //act
        var exception = Record.Exception(() => userCalendar.EditEvent(Guid.NewGuid(), "test_title",
            new TimeOnly(12,00), null, "test_action"));
        
        //assert
        exception.ShouldBeOfType<EventNotFoundException>();
    }

    [Fact]
    public void EditEvent_GivenInvalidEventTypeIdAndArgumentsForCalendarEvent_ShouldThrowInvalidEventTypeIdException()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var id = Guid.NewGuid();
        userCalendar.AddEvent(id, "test_important_date");
        
        //act
        var exception = Record.Exception(() => userCalendar.EditEvent(id, "test_title",
            new TimeOnly(12,00), null, "test_action"));
        
        //assert
        exception.ShouldBeOfType<InvalidEventTypeIdException>();
    }
    
    [Fact]
    public void EditEvent_GivenArgumentsForMeeting_ShouldEditMeeting()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var id = Guid.NewGuid();
        userCalendar.AddEvent(id, "test_meeting_title", new TimeOnly(12, 00),
            null, null, null, "test_meeting_place");
        var newMeetingTitle = "new_meeting_title";
        var newMeetingTimeFrom = new TimeOnly(13, 00);
        var newMeetingTimeTo = new TimeOnly(14, 00);
        var newMeetingPlatform = "new_meeting_platform";
        var newMeetingUri = "new_meeting_uri";
        
        //act
        userCalendar.EditEvent(id, newMeetingTitle, newMeetingTimeFrom, newMeetingTimeTo,
            newMeetingPlatform, newMeetingUri, null);
        
        //assert
        var meeting = userCalendar
            .Events
            .First(x => x.Id.Value == id);
        ((Meeting)meeting).Title.Value.ShouldBe(newMeetingTitle);
        ((Meeting)meeting).MeetingTimeSpan.From.ShouldBe(newMeetingTimeFrom);
        ((Meeting)meeting).MeetingTimeSpan.To.ShouldBe(newMeetingTimeTo);
        ((Meeting)meeting).Address.Platform.ShouldBe(newMeetingPlatform);
        ((Meeting)meeting).Address.Uri.ShouldBe(newMeetingUri);
        ((Meeting)meeting).Address.Place.ShouldBeNull();
    }

    [Fact]
    public void EditEvent_GivenNotExistingIdAndArgumentsForMeeting_ShouldThrowEventNotExistsException()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        
        //act
        var exception = Record.Exception(() => userCalendar.EditEvent(Guid.NewGuid(), "test_title", new TimeOnly(12,00),
            null, "test_platform", "test_uri", null));
        
        //assert
        exception.ShouldBeOfType<EventNotFoundException>();
    }

    [Fact]
    public void EditEvent_GivenInvalidEventTypeIdAndArgumentsForMeeting_ShouldThrowInvalidEventTypeIdException()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var id = Guid.NewGuid();
        userCalendar.AddEvent(id, "test_important_date");
        
        //act
        var exception = Record.Exception(() => userCalendar.EditEvent(id, "test_title", new TimeOnly(12,00),
            null, "test_platform", "test_uri", null));
        
        //assert
        exception.ShouldBeOfType<InvalidEventTypeIdException>();
    }

    [Fact]
    public void RemoveEvent_GivenNotExistingEvent_ShouldThrowEventNotFoundException()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        
        //act
        var exception = Record.Exception(() => userCalendar.RemoveEvent(Guid.NewGuid()));
        
        //assert
        exception.ShouldBeOfType<EventNotFoundException>();
    }
    
    [Fact]
    public void RemoveEvent_GivenExistingEvent_ShouldRemoveEvent()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var id = Guid.NewGuid();
        userCalendar.AddEvent(id, "test_important_date");
        
        //act
        userCalendar.RemoveEvent(Guid.NewGuid());
        
        //assert
        userCalendar.Events.Any(x => x.Id.Value == id).ShouldBeFalse();
    }
}