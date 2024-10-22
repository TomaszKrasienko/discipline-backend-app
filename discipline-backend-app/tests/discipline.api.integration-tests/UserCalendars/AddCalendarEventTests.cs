using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Features.UsersCalendars;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;
using discipline.domain.SharedKernel.TypeIdentifiers;
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
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var command = new AddCalendarEventCommand(DateOnly.FromDateTime(DateTime.Now), UserId.New(), new EventId(Ulid.Empty), "test_title",
            new TimeOnly(15,00), null, "test_action");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-calendar-event", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var userCalendar = await TestAppDb.GetCollection<UserCalendarDocument>()
            .Find(x => x.Day == command.Day && x.UserId == user.Id.Value )
            .FirstOrDefaultAsync();

        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Ulid.Empty.ToString());
        
        userCalendar.ShouldNotBeNull();
        var eventDocument = userCalendar.Events.FirstOrDefault(x => x.Id.ToString() == resourceId);
        eventDocument.ShouldBeOfType<CalendarEventDocument>();
        eventDocument.Title.ShouldBe(command.Title);
    }
    
    [Fact]
    public async Task AddCalendarEvent_GivenExistingUserCalendar_ShouldReturn201CreatedStatusCodeUpdateUserCalendarWithCalendarEvent()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var userCalendar = UserCalendarFactory.Get();
        var @event = MeetingFactory.GetInUserCalender(userCalendar);
        var userCalendarDocument = userCalendar.AsDocument();
        userCalendarDocument.UserId = user.Id.Value; 
        await TestAppDb.GetCollection<UserCalendarDocument>().InsertOneAsync(userCalendarDocument);
        var command = new AddCalendarEventCommand(DateOnly.FromDateTime(DateTime.Now), new UserId(Ulid.Empty),
            new EventId(Ulid.Empty), "test_title",
            new TimeOnly(15,00), null, "test_action");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-calendar-event", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var updatedUserCalendar = await TestAppDb.GetCollection<UserCalendarDocument>()
            .Find(x => x.Day == command.Day && x.UserId == user.Id.Value )
            .FirstOrDefaultAsync();

        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Ulid.Empty.ToString());
        
        updatedUserCalendar.ShouldNotBeNull();
        var eventDocument = updatedUserCalendar.Events.FirstOrDefault(x => x.Id == resourceId);
        eventDocument.ShouldBeOfType<CalendarEventDocument>();
        eventDocument.Title.ShouldBe(command.Title);
    }
    
    [Fact]
    public async Task AddCalendarEvent_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new AddCalendarEventCommand(DateOnly.FromDateTime(DateTime.Now), new UserId(Ulid.Empty),
            new EventId(Ulid.Empty), "test_title", new TimeOnly(15,00), null, "test_action");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-calendar-event", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task AddCalendarEvent_GivenAuthorizedWithoutPickedSubscription_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        var command = new AddCalendarEventCommand(DateOnly.FromDateTime(DateTime.Now), 
            new UserId(Ulid.Empty), new EventId(Ulid.Empty), "test_title", new TimeOnly(15,00), null, "test_action");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-calendar-event", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task AddCalendarEvent_GivenEmptyTitle_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        var command = new AddCalendarEventCommand(DateOnly.FromDateTime(DateTime.Now), new UserId(Ulid.Empty), 
            new EventId(Ulid.Empty), string.Empty, new TimeOnly(15,00), null, "test_action");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-calendar-event", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}