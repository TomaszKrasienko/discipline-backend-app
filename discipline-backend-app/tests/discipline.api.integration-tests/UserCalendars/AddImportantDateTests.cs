using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Domain.UsersCalendars.Entities;
using discipline.application.Features.UsersCalendars;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;
using discipline.tests.shared.Entities;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.UserCalendars;

[Collection("integration-tests")]
public sealed class AddImportantDateTests : BaseTestsController
{
    [Fact]
    public async Task AddImportantDate_GivenNotExistingUserCalendar_ShouldReturn201CreatedStatusCodeAddUserCalendarWithImportantDateEvent()
    {
        //arrange
        var command = new AddImportantDateCommand(DateOnly.FromDateTime(DateTime.Now), Guid.Empty, "test_title");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-important-date", command);
        
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
        eventDocument.ShouldBeOfType<ImportantDateDocument>();
        eventDocument.Title.ShouldBe(command.Title);
    }
    
    [Fact]
    public async Task AddImportantDate_GivenExistingUserCalendar_ShouldReturn201CreatedStatusCodeUpdateUserCalendarWithImportantDateEvent()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var @event = MeetingFactory.GetInUserCalender(userCalendar);
        await TestAppDb.GetCollection<UserCalendarDocument>().InsertOneAsync(userCalendar.AsDocument());
        var command = new AddImportantDateCommand(userCalendar.Day, Guid.Empty, "test_title");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-important-date", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);
        
        var updatedUserCalendar = await TestAppDb.GetCollection<UserCalendarDocument>()
            .Find(x => x.Day == command.Day)
            .FirstOrDefaultAsync();
        var eventDocument = updatedUserCalendar.Events.FirstOrDefault(x => x.Id == resourceId);
        eventDocument.ShouldBeOfType<ImportantDateDocument>();
        eventDocument.Title.ShouldBe(command.Title);
    }
    
    [Fact]
    public async Task AddImportantDate_GivenEmptyTitle_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        var command = new AddImportantDateCommand(DateOnly.FromDateTime(DateTime.Now), Guid.Empty, string.Empty);
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-important-date", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}