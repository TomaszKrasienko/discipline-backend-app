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
public sealed class EditCalendarEventTests : BaseTestsController
{
    [Fact]
    public async Task EditCalendarEvent_GivenExistingUserCalendarForUser_ShouldReturn200OkStatusCodeAndUpdateEvent()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var userDocument = user.AsDocument();
        userDocument.Id = user.Id.ToString();
        
        var userCalendar = UserCalendarFactory.Get();
        var eventId = EventId.New();
        userCalendar.AddEvent(eventId, "test_title", new TimeOnly(12, 00), null,
            "test_action");
        var userCalendarDocument = userCalendar.AsDocument();
        userCalendarDocument.UserId = user.Id.Value;
        await TestAppDb.GetCollection<UserCalendarDocument>().InsertOneAsync(userCalendarDocument);

        var command = new EditCalendarEventCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), "new_test_title",
            new TimeOnly(13, 00), null, "new_test_action");
        
        //act
        var result = await HttpClient.PutAsJsonAsync($"user-calendar/edit-calendar-event/{eventId}",
            command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedUserCalendar = await TestAppDb.GetCollection<UserCalendarDocument>()
            .Find(x => x.Events.Any(y => y.Id == eventId.ToString()))
            .FirstAsync();
        var @event = updatedUserCalendar.Events.First(x => x.Id == eventId.ToString());
        ((CalendarEventDocument)@event!).Title.ShouldBe(command.Title);
        ((CalendarEventDocument)@event!).TimeFrom.ShouldBe(command.TimeFrom);
        ((CalendarEventDocument)@event!).TimeTo.ShouldBe(command.TimeTo);
        ((CalendarEventDocument)@event!).Action.ShouldBe(command.Action);
    }
    
    [Fact]
    public async Task EditCalendarEvent_GivenNotExistingUserCalendarForUser_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var command = new EditCalendarEventCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), "new_test_title",
            new TimeOnly(13, 00), null, "new_test_action");
        
        //act
        var result = await HttpClient.PutAsJsonAsync($"user-calendar/edit-calendar-event/{Ulid.NewUlid()}",
            command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    

    [Fact]
    public async Task EditCalendarEvent_Unauthorized_ShouldReturnStatusCode401Unauthorized()
    {
        //arrange
        var command = new EditCalendarEventCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), "new_test_title",
            new TimeOnly(13, 00), null, "new_test_action");
        
        //act
        var result = await HttpClient.PutAsJsonAsync($"user-calendar/edit-calendar-event/{Ulid.NewUlid()}",
            command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task EditCalendarEvent_GivenAuthorizedWithoutPickedSubscription_ShouldReturnStatusCode403Forbidden()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        var command = new EditCalendarEventCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), "new_test_title",
            new TimeOnly(13, 00), null, "new_test_action");
        
        //act
        var result = await HttpClient.PutAsJsonAsync($"user-calendar/edit-calendar-event/{Ulid.NewUlid()}",
            command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task EditCalendarEvent_GivenInvalidCommand_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var command = new EditCalendarEventCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty),string.Empty,
            new TimeOnly(13, 00), null, "new_test_action");
        
        //act
        var result = await HttpClient.PutAsJsonAsync($"user-calendar/edit-calendar-event/{Ulid.NewUlid()}",
            command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}