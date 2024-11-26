using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Features.UsersCalendars;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.infrastructure.DAL.Documents.Mappers;
using discipline.infrastructure.DAL.Documents.UsersCalendar;
using discipline.tests.shared.Entities;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.UserCalendars;

[Collection("integration-tests")]
public sealed class EditImportantDateTests : BaseTestsController
{
    [Fact]
    public async Task EditImportantDate_GivenExistingUserCalendarForUser_ShouldReturn200OkStatusCodeAndUpdateEvent()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var userDocument = user.AsDocument();
        userDocument.Id = user.Id.ToString();
        
        var userCalendar = UserCalendarFactory.Get();
        var eventId = EventId.New();
        userCalendar.AddEvent(eventId, "test_title");
        var userCalendarDocument = userCalendar.AsDocument();
        userCalendarDocument.UserId = user.Id.ToString();
        await TestAppDb.GetCollection<UserCalendarDocument>().InsertOneAsync(userCalendarDocument);

        var command = new EditImportantDateCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), "new_test_title");
        
        //act
        var result = await HttpClient.PutAsJsonAsync($"user-calendar/edit-important-date/{eventId}",
            command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedUserCalendar = await TestAppDb.GetCollection<UserCalendarDocument>()
            .Find(x => x.Events.Any(y => y.Id == eventId.ToString()))
            .FirstAsync();
        var @event = updatedUserCalendar.Events.First(x => x.Id == eventId.ToString());
        ((ImportantDateDocument)@event!).Title.ShouldBe(command.Title);
    }
    
    [Fact]
    public async Task EditImportantDate_GivenNotExistingUserCalendarForUser_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        var command = new EditImportantDateCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), "new_test_title");
        
        //act
        var result = await HttpClient.PutAsJsonAsync($"user-calendar/edit-important-date/{Ulid.NewUlid()}",
            command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    

    [Fact]
    public async Task EditImportantDate_Unauthorized_ShouldReturnStatusCode401Unauthorized()
    {
        //arrange
        var command = new EditImportantDateCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), "new_test_title");
        
        //act
        var result = await HttpClient.PutAsJsonAsync($"user-calendar/edit-important-date/{Ulid.NewUlid()}",
            command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task EditImportantDate_GivenAuthorizedWithoutPickedSubscription_ShouldReturnStatusCode403Forbidden()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        var command = new EditImportantDateCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), "new_test_title");
        
        //act
        var result = await HttpClient.PutAsJsonAsync($"user-calendar/edit-important-date/{Ulid.NewUlid()}",
            command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task EditImportantDate_GivenInvalidCommand_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        var command = new EditImportantDateCommand(new UserId(Ulid.Empty), new EventId(Ulid.Empty), string.Empty);
        
        //act
        var result = await HttpClient.PutAsJsonAsync($"user-calendar/edit-important-date/{Ulid.Empty}",
            command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}