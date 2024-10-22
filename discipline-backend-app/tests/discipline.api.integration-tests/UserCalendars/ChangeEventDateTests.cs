using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Features.UsersCalendars;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.Entities;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.UserCalendars;

[Collection("integration-tests")]
public sealed class ChangeEventDateTests : BaseTestsController
{
    [Fact]
    public async Task ChangeEventDate_GivenExistingUserCalendars_ShouldReturn200OkStatusCodeAndChangeEventUserCalendar()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var oldUserCalendar = UserCalendar.Create(UserCalendarId.New(), new DateOnly(2024, 1, 1), user.Id);
        var newUserCalendar = UserCalendar.Create(UserCalendarId.New(), new DateOnly(2024, 1, 3), user.Id);

        var eventId = EventId.New();
        oldUserCalendar.AddEvent(eventId, "test_event_title", new TimeOnly(12, 00), null, "test_action");
        await TestAppDb.GetCollection<UserCalendarDocument>()
            .InsertOneAsync(oldUserCalendar.AsDocument());
        await TestAppDb.GetCollection<UserCalendarDocument>()
            .InsertOneAsync(newUserCalendar.AsDocument());
        
        var command = new ChangeEventDateCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), newUserCalendar.Day);

        
        //act
        var response = await HttpClient.PatchAsJsonAsync($"user-calendar/event/{eventId}/change-event-date", command);

        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var newUpdatedUserCalendar = await TestAppDb.GetCollection<UserCalendarDocument>()
            .Find(x => x.Day == command.NewDate)
            .FirstOrDefaultAsync();
        newUpdatedUserCalendar.Events.Any(x => x.Id == eventId.ToString()).ShouldBeTrue();
        
        var oldUpdatedUserCalendar = await TestAppDb.GetCollection<UserCalendarDocument>()
            .Find(x => x.Day == oldUserCalendar.Day)
            .FirstOrDefaultAsync();
        oldUpdatedUserCalendar.Events.Any(x => x.Id == eventId.ToString()).ShouldBeFalse();
    }
    
    [Fact]
    public async Task ChangeEventDate_GivenExistingPresentUserCalendarAndNotExistingNewUserCalendar_ShouldReturn200OkStatusCodeAndAddNewUserCalendarAndChangeEventUserCalendar()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var oldUserCalendar = UserCalendar.Create(UserCalendarId.New(), new DateOnly(2024, 1, 1), user.Id);

        var eventId = EventId.New();
        oldUserCalendar.AddEvent(eventId, "test_event_title", new TimeOnly(12, 00), null, "test_action");
        await TestAppDb.GetCollection<UserCalendarDocument>()
            .InsertOneAsync(oldUserCalendar.AsDocument());
        var newDate = oldUserCalendar.Day.Value.AddDays(5);
        
        var command = new ChangeEventDateCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), newDate);
        
        //act
        var response = await HttpClient.PatchAsJsonAsync($"user-calendar/event/{eventId}/change-event-date", command);

        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var newUpdatedUserCalendar = await TestAppDb.GetCollection<UserCalendarDocument>()
            .Find(x => x.Day == command.NewDate)
            .FirstOrDefaultAsync();
        newUpdatedUserCalendar.Events.Any(x => x.Id == eventId.ToString()).ShouldBeTrue();
        
        var oldUpdatedUserCalendar = await TestAppDb.GetCollection<UserCalendarDocument>()
            .Find(x => x.Day == oldUserCalendar.Day)
            .FirstOrDefaultAsync();
        oldUpdatedUserCalendar.Events.Any(x => x.Id == eventId.ToString()).ShouldBeFalse();
    }
    
    [Fact]
    public async Task ChangeEventDate_GivenNotEvent_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        var command = new ChangeEventDateCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), new DateOnly(2024,1,1));
        
        //act
        var response = await HttpClient.PatchAsJsonAsync($"user-calendar/event/{Guid.NewGuid()}/change-event-date", command);

        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task ChangeEventDate_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new ChangeEventDateCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), new DateOnly(2021,1,1));
        
        //act
        var response = await HttpClient.PatchAsJsonAsync($"user-calendar/event/{Guid.NewGuid()}/change-event-date", command);

        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task ChangeEventDate_GivenAuthorizedWithoutPickedSubscription_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        var command = new ChangeEventDateCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), new DateOnly(2021,1,1));
        
        //act
        var response = await HttpClient.PatchAsJsonAsync($"user-calendar/event/{Guid.NewGuid()}/change-event-date", command);

        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task ChangeEventDate_GivenEmptyEventId_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        var command = new ChangeEventDateCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), new DateOnly(2021,1,1));
        
        //act
        var response = await HttpClient.PatchAsJsonAsync($"user-calendar/event/{Guid.Empty}/change-event-date", command);

        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}