using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.DTOs;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;
using discipline.tests.shared.Documents;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.UserCalendars;

[Collection("integration-tests")]
public sealed class BrowseUserCalendarTests : BaseTestsController
{
    [Fact]
    public async Task BrowseUserCalendar_GivenExistingUserCalendarWithEvents_ShouldReturn200OkStatusCodeWithUserCalendarDto()
    {
        //arrange 
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var importantDateDocument = ImportantDateDocumentFactory.Get();
        var calendarEventDocument = CalendarEventDocumentFactory.Get(true);
        var meetingDocument = MeetingDocumentFactory.Get(true, true);
        var userCalendarDocument = UserCalendarDocumentFactory.Get([importantDateDocument, calendarEventDocument, meetingDocument]);
        userCalendarDocument.UserId = user.Id.Value;
        
        await TestAppDb
            .GetCollection<UserCalendarDocument>()
            .InsertOneAsync(userCalendarDocument);
        
        //act
        var result = await HttpClient.GetFromJsonAsync<UserCalendarDto>($"user-calendar/{userCalendarDocument.Day:yyyy-MM-dd}");
        
        //assert
        result.Day.ShouldBe(userCalendarDocument.Day);
        result.ImportantDates.Any(x
            => x.Id == importantDateDocument.Id
               && x.Title == importantDateDocument.Title).ShouldBeTrue();
        
        result.CalendarEvents.Any(x
            => x.Id == calendarEventDocument.Id
               && x.Title == calendarEventDocument.Title
               && x.TimeFrom == calendarEventDocument.TimeFrom
               && x.TimeTo == calendarEventDocument.TimeTo
               && x.Action == calendarEventDocument.Action).ShouldBeTrue();

        result.Meetings.Any(x
            => x.Id == meetingDocument.Id
               && x.Title == meetingDocument.Title
               && x.TimeFrom == meetingDocument.TimeFrom
               && x.TimeTo == meetingDocument.TimeTo
               && x.Platform == meetingDocument.Platform
               && x.Uri == meetingDocument.Uri
               && x.Place == meetingDocument.Place).ShouldBeTrue();
    }
    
    [Fact]
    public async Task BrowseUserCalendar_GivenNotExistingUserCalendar_ShouldReturn204NoContentStatusCode()
    {
        //act
        await AuthorizeWithFreeSubscriptionPicked();
        var response = await HttpClient.GetAsync($"user-calendar/{new DateOnly(2024, 1, 1):yyyy-MM-dd}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task BrowseUserCalendar_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var response = await HttpClient.GetAsync($"user-calendar/{new DateOnly(2024, 1, 1):yyyy-MM-dd}");

        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task BrowseUserCalendar_GivenAuthorizedWithoutPickedSubscription_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        
        //act
        var response = await HttpClient.GetAsync($"user-calendar/{new DateOnly(2024, 1, 1):yyyy-MM-dd}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}