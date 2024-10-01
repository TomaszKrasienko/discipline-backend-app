using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Features.UsersCalendars;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;
using discipline.domain.UsersCalendars.Entities;
using discipline.tests.shared.Entities;
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
        var oldUserCalendar = UserCalendar.Create(new DateOnly(2024, 1, 1), user.Id);
        var newUserCalendar = UserCalendar.Create(new DateOnly(2024, 1, 3), user.Id);

        var eventId = Guid.NewGuid();
        oldUserCalendar.AddEvent(eventId, "test_event_title", new TimeOnly(12, 00), null, "test_action");
        await TestAppDb.GetCollection<UserCalendarDocument>()
            .InsertOneAsync(oldUserCalendar.AsDocument());
        await TestAppDb.GetCollection<UserCalendarDocument>()
            .InsertOneAsync(newUserCalendar.AsDocument());

        var command = new ChangeEventDateCommand(Guid.Empty, Guid.Empty, newUserCalendar.Day);
        
        //act
        var response = await HttpClient.PatchAsJsonAsync($"user-calendar/event/{eventId}/change-event-date", command);

        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var newUpdatedUserCalendar = await TestAppDb.GetCollection<UserCalendarDocument>()
            .Find(x => x.Day == command.NewDate)
            .FirstOrDefaultAsync();
        newUpdatedUserCalendar.Events.Any(x => x.Id == eventId).ShouldBeTrue();
        
        var oldUpdatedUserCalendar = await TestAppDb.GetCollection<UserCalendarDocument>()
            .Find(x => x.Day == oldUserCalendar.Day)
            .FirstOrDefaultAsync();
        oldUpdatedUserCalendar.Events.Any(x => x.Id == eventId).ShouldBeFalse();
    }
}