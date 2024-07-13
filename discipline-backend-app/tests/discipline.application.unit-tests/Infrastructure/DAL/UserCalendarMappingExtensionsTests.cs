using discipline.application.Domain.UsersCalendars.Entities;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Infrastructure.DAL;

public sealed class UserCalendarMappingExtensionsTests
{
    [Fact]
    public void AsDocument_GivenUserCalendarWithAllTypesOfEvents_ShouldReturnUserCalendarDocumentWithEventsDocuments()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var importantDate = ImportantDateFactory.GetInUserCalender(userCalendar);
        var calendarEvent = CalendarEventFactory.GetInUserCalender(userCalendar);
        var meeting = MeetingFactory.GetInUserCalender(userCalendar);
        
        //act
        var result = userCalendar.AsDocument();
        
        //assert
        result.Day.ShouldBe(userCalendar.Day.Value);
        var importantDateDocument = result.Events.SingleOrDefault(x => x.Id == importantDate.Id.Value);
        importantDateDocument.ShouldBeOfType<ImportantDateDocument>();
        ((ImportantDateDocument)importantDateDocument!).Title.ShouldBe(importantDate.Title);
        
        
        var calendarEventDocument = result.Events.SingleOrDefault(x => x.Id == calendarEvent.Id.Value);
        calendarEventDocument.ShouldBeOfType<CalendarEventDocument>();
        ((CalendarEventDocument)calendarEventDocument!).Title.ShouldBe(calendarEvent.Title.Value);
        ((CalendarEventDocument)calendarEventDocument!).TimeFrom.ShouldBe(calendarEvent.MeetingTimeSpan.From);
        ((CalendarEventDocument)calendarEventDocument!).TimeTo.ShouldBe(calendarEvent.MeetingTimeSpan.To);
        ((CalendarEventDocument)calendarEventDocument!).Action.ShouldBe(calendarEvent.Action.Value);
        
        var meetingDocument = result.Events.SingleOrDefault(x => x.Id == meeting.Id.Value);
        meetingDocument.ShouldBeOfType<MeetingDocument>();
        ((MeetingDocument)meetingDocument).Title.ShouldBe(meeting.Title.Value);
        ((MeetingDocument)meetingDocument).TimeFrom.ShouldBe(meeting.MeetingTimeSpan.From);
        ((MeetingDocument)meetingDocument).TimeTo.ShouldBe(meeting.MeetingTimeSpan.To);
        ((MeetingDocument)meetingDocument).Place.ShouldBe(meeting.Address.Place);
        ((MeetingDocument)meetingDocument).Platform.ShouldBe(meeting.Address.Platform);
        ((MeetingDocument)meetingDocument).Uri.ShouldBe(meeting.Address.Uri);
    }
    
        [Fact]
    public void AsDocument_GivenUserCalendarWithoutEvents_ShouldReturnUserCalendarDocument()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        
        //act
        var result = userCalendar.AsDocument();
        
        //assert
        result.Day.ShouldBe(userCalendar.Day.Value);
        result.Events.ShouldBeEmpty();
    }
    
}