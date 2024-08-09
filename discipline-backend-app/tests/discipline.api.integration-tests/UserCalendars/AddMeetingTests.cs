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
public class AddMeetingTests : BaseTestsController
{
    [Fact]
    public async Task AddMeeting_GivenNotExistingUserCalendar_ShouldReturn201CreatedStatusCodeAddUserCalendarWithMeeting()
    {
        //arrange
        var command = new AddMeetingCommand(DateOnly.FromDateTime(DateTime.Now), Guid.Empty, "test_title",
            new TimeOnly(15,00), null, "test_platform", "test_uri", null);
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-meeting", command);
        
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
        eventDocument.ShouldBeOfType<MeetingDocument>();
        eventDocument.Title.ShouldBe(command.Title);
    }
    
    [Fact]
    public async Task AddMeeting_GivenExistingUserCalendar_ShouldReturn201CreatedStatusCodeUpdateUserCalendarWithMeeting()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var @event = MeetingFactory.GetInUserCalender(userCalendar);
        await TestAppDb.GetCollection<UserCalendarDocument>().InsertOneAsync(userCalendar.AsDocument());
        var command = new AddMeetingCommand(DateOnly.FromDateTime(DateTime.Now), Guid.Empty, "test_title",
            new TimeOnly(15,00), null, null, null, "test_place");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-meeting", command);
        
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
        eventDocument.ShouldBeOfType<MeetingDocument>();
        eventDocument.Title.ShouldBe(command.Title);
    }
    
    [Fact]
    public async Task AddMeeting_GivenEmptyTitle_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        var command = new AddMeetingCommand(DateOnly.FromDateTime(DateTime.Now), Guid.Empty, string.Empty,
            new TimeOnly(15,00), null, "test_platform", "test_uri", null);
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-meeting", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}