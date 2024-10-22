using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Features.UsersCalendars;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.Entities;
using discipline.tests.shared.Entities;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.UserCalendars;

[Collection("integration-tests")]
public sealed class EditMeetingTests : BaseTestsController
{
    [Fact]
    public async Task EditMeeting_GivenExistingUserCalendarForUser_ShouldReturn200OkStatusCodeAndUpdateEvent()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var userDocument = user.AsDocument();
        userDocument.Id = user.Id.ToString();
        
        var userCalendar = UserCalendarFactory.Get();
        var eventId = EventId.New();
        userCalendar.AddEvent(eventId, "test_meeting_title", new TimeOnly(12, 00), new TimeOnly(13,00),
            "test_platform", "test_uri", null);
        var userCalendarDocument = userCalendar.AsDocument();
        userCalendarDocument.UserId = user.Id.ToString();
        await TestAppDb.GetCollection<UserCalendarDocument>().InsertOneAsync(userCalendarDocument);

        var command = new EditMeetingCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), "new_test_title",
            new TimeOnly(13, 00), null, "new_test_platform", "new_test_uri", null);
        
        //act
        var result = await HttpClient.PutAsJsonAsync($"user-calendar/edit-meeting/{eventId}",
            command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedUserCalendar = await TestAppDb.GetCollection<UserCalendarDocument>()
            .Find(x => x.Events.Any(y => y.Id == eventId.ToString()))
            .FirstAsync();
        var @event = updatedUserCalendar.Events.First(x => x.Id == eventId.ToString());
        ((MeetingDocument)@event!).Title.ShouldBe(command.Title);
        ((MeetingDocument)@event!).TimeFrom.ShouldBe(command.TimeFrom);
        ((MeetingDocument)@event!).TimeTo.ShouldBe(command.TimeTo);
        ((MeetingDocument)@event!).Platform.ShouldBe(command.Platform);
        ((MeetingDocument)@event!).Uri.ShouldBe(command.Uri);
        ((MeetingDocument)@event!).Place.ShouldBe(command.Place);
    }
    
    [Fact]
    public async Task EditMeeting_GivenNotExistingUserCalendarForUser_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        var command = new EditMeetingCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), "new_test_title",
            new TimeOnly(13, 00), null, "new_test_platform", "new_test_uri", null);
        
        //act
        var result = await HttpClient.PutAsJsonAsync($"user-calendar/edit-meeting/{Ulid.NewUlid()}",
            command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    

    [Fact]
    public async Task EditMeeting_Unauthorized_ShouldReturnStatusCode401Unauthorized()
    {
        //arrange
        var command = new EditMeetingCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), "new_test_title",
        new TimeOnly(13, 00), null, "new_test_platform", "new_test_uri", null);
        
        //act
        var result = await HttpClient.PutAsJsonAsync($"user-calendar/edit-meeting/{Ulid.NewUlid()}",
            command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task EditMeeting_GivenAuthorizedWithoutPickedSubscription_ShouldReturnStatusCode403Forbidden()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        var command = new EditMeetingCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), "new_test_title",
            new TimeOnly(13, 00), null, "new_test_platform", "new_test_uri", null);
        
        //act
        var result = await HttpClient.PutAsJsonAsync($"user-calendar/edit-meeting/{Ulid.NewUlid()}",
            command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task EditMeeting_GivenInvalidCommand_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        var command = new EditMeetingCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), string.Empty,
            new TimeOnly(13, 00), null, "new_test_platform", "new_test_uri", null);
        
        //act
        var result = await HttpClient.PutAsJsonAsync($"user-calendar/edit-meeting/{Ulid.NewUlid()}",
            command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}