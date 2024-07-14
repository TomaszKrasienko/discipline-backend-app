using discipline.application.Domain.UsersCalendars.Entities;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;
using discipline.tests.shared.Documents;
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

    [Fact]
    public void AsEntity_GivenUserCalendarDocumentWithAllTypesOfEventDocuments_ShouldReturnUserCalendarWithEvents()
    {
        //arrange
        var importantDateDocument = ImportantDateDocumentFactory.Get();
        var calendarEventDocument = CalendarEventDocumentFactory.Get(true);
        var meetingDocument = MeetingDocumentFactory.Get(true, true);
        var userCalendarDocument = UserCalendarDocumentFactory.Get([importantDateDocument, calendarEventDocument, meetingDocument]);
        
        //act
        var result = userCalendarDocument.AsEntity();
        
        //assert
        result.Day.Value.ShouldBe(userCalendarDocument.Day);
        var importantDate = result.Events.SingleOrDefault(x => x.Id.Value == importantDateDocument.Id);
        importantDate.ShouldBeOfType<ImportantDate>();
        ((ImportantDate)importantDate).Title.Value.ShouldBe(importantDateDocument.Title);
        
        
        var calendarEvent = result.Events.SingleOrDefault(x => x.Id.Value == calendarEventDocument.Id);
        calendarEvent.ShouldBeOfType<CalendarEvent>();
        ((CalendarEvent)calendarEvent).Title.Value.ShouldBe(calendarEventDocument.Title);
        ((CalendarEvent)calendarEvent).MeetingTimeSpan.From.ShouldBe(calendarEventDocument.TimeFrom);
        ((CalendarEvent)calendarEvent).MeetingTimeSpan.To.ShouldBe(calendarEventDocument.TimeTo);
        ((CalendarEvent)calendarEvent).Action.Value.ShouldBe(calendarEventDocument.Action);
        
        var meeting = result.Events.SingleOrDefault(x => x.Id.Value == meetingDocument.Id);
        meeting.ShouldBeOfType<Meeting>();
        ((Meeting)meeting).Title.Value.ShouldBe(meetingDocument.Title);
        ((Meeting)meeting).MeetingTimeSpan.From.ShouldBe(meetingDocument.TimeFrom);
        ((Meeting)meeting).MeetingTimeSpan.To.ShouldBe(meetingDocument.TimeTo);
        ((Meeting)meeting).Address.Place.ShouldBe(meetingDocument.Place);
        ((Meeting)meeting).Address.Platform.ShouldBe(meetingDocument.Platform);
        ((Meeting)meeting).Address.Uri.ShouldBe(meetingDocument.Uri);
    }

    [Fact]
    public void AsEntity_GivenUserCalendarDocumentWithoutEventDocuments_ShouldReturnUserCalendarWithoutEvents()
    {
        //arrange
        var userCalendarDocument = UserCalendarDocumentFactory.Get([]);
        
        //act
        var result = userCalendarDocument.AsEntity();
        
        //assert
        result.Day.Value.ShouldBe(userCalendarDocument.Day);
        result.Events.ShouldBeEmpty();
    }

    [Fact]
    public void AsDto_GivenUserCalendarDocumentWithAllEventTypes_ShouldReturnUserCalendarDtoWithEventsDto()
    {
        //arrange
        var importantDateDocument = ImportantDateDocumentFactory.Get();
        var calendarEventDocument = CalendarEventDocumentFactory.Get(true);
        var meetingDocument = MeetingDocumentFactory.Get(true, true);
        var userCalendarDocument = UserCalendarDocumentFactory.Get([importantDateDocument, calendarEventDocument, meetingDocument]);
        
        //act
        var result = userCalendarDocument.AsDto();
        
        //assert
        result.Day.ShouldBe(userCalendarDocument.Day);
        result.ImportantDates.Any(x
            => x.Id == importantDateDocument.Id
            && x.Title == importantDateDocument.Title).ShouldBeTrue();
        
        // var calendarEvent = result.Events.SingleOrDefault(x => x.Id.Value == calendarEventDocument.Id);
        // calendarEvent.ShouldBeOfType<CalendarEvent>();
        // ((CalendarEvent)calendarEvent).Title.Value.ShouldBe(calendarEventDocument.Title);
        // ((CalendarEvent)calendarEvent).MeetingTimeSpan.From.ShouldBe(calendarEventDocument.TimeFrom);
        // ((CalendarEvent)calendarEvent).MeetingTimeSpan.To.ShouldBe(calendarEventDocument.TimeTo);
        // ((CalendarEvent)calendarEvent).Action.Value.ShouldBe(calendarEventDocument.Action);
        //
        // var meeting = result.Events.SingleOrDefault(x => x.Id.Value == meetingDocument.Id);
        // meeting.ShouldBeOfType<Meeting>();
        // ((Meeting)meeting).Title.Value.ShouldBe(meetingDocument.Title);
        // ((Meeting)meeting).MeetingTimeSpan.From.ShouldBe(meetingDocument.TimeFrom);
        // ((Meeting)meeting).MeetingTimeSpan.To.ShouldBe(meetingDocument.TimeTo);
        // ((Meeting)meeting).Address.Place.ShouldBe(meetingDocument.Place);
        // ((Meeting)meeting).Address.Platform.ShouldBe(meetingDocument.Platform);
        // ((Meeting)meeting).Address.Uri.ShouldBe(meetingDocument.Uri);
    }
}