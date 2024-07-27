using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Features.UsersCalendars;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;
using discipline.tests.shared.Entities;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.UserCalendars; 

[Collection("integration-tests")]
public sealed class AddCalendarEventTests : BaseTestsController
{
    [Fact]
    public async Task AddCalendarEvent_GivenNotExistingUserCalendar_ShouldReturn201CreatedStatusCodeAddUserCalendarWithCalendarEvent()
    {
        //arrange
        var command = new AddCalendarEventCommand(DateOnly.FromDateTime(DateTime.Now), Guid.Empty, "test_title",
            new TimeOnly(15,00), null, "test_action");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-calendar-event", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var userCalendar = await TestAppDb.GetCollection<UserCalendarDocument>()
            .Find(x => x.Day == command.Day)
            .FirstOrDefaultAsync();

        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);
        
        userCalendar.ShouldNotBeNull();
        var eventDocument = userCalendar.Events.FirstOrDefault(x => x.Id == resourceId);
        eventDocument.ShouldBeOfType<CalendarEventDocument>();
        eventDocument.Title.ShouldBe(command.Title);
    }
    
    [Fact]
    public async Task AddCalendarEvent_GivenExistingUserCalendar_ShouldReturn201CreatedStatusCodeUpdateUserCalendarWithCalendarEvent()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var @event = MeetingFactory.GetInUserCalender(userCalendar);
        await TestAppDb.GetCollection<UserCalendarDocument>().InsertOneAsync(userCalendar.AsDocument());
        var command = new AddCalendarEventCommand(DateOnly.FromDateTime(DateTime.Now), Guid.Empty, "test_title",
            new TimeOnly(15,00), null, "test_action");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-calendar-event", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var updatedUserCalendar = await TestAppDb.GetCollection<UserCalendarDocument>()
            .Find(x => x.Day == command.Day)
            .FirstOrDefaultAsync();

        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);
        
        updatedUserCalendar.ShouldNotBeNull();
        var eventDocument = updatedUserCalendar.Events.FirstOrDefault(x => x.Id == resourceId);
        eventDocument.ShouldBeOfType<CalendarEventDocument>();
        eventDocument.Title.ShouldBe(command.Title);
    }
    
    [Fact]
    public async Task AddCalendarEvent_GivenEmptyTitle_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        var command = new AddCalendarEventCommand(DateOnly.FromDateTime(DateTime.Now), Guid.Empty, string.Empty,
            new TimeOnly(15,00), null, "test_action");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-calendar-event", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}