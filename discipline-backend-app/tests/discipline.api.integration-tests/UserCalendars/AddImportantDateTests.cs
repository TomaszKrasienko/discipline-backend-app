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

public sealed class AddImportantDateTests : BaseTestsController
{
    [Fact]
    public async Task AddImportantDate_GivenNotExistingUserCalendar_ShouldReturn201CreatedStatusCodeAddUserCalendarWithImportantDateEvent()
    {
        //arrange
        var command = new AddImportantDateCommand(DateOnly.FromDateTime(DateTime.Now), Guid.NewGuid(), "test_title");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-important-date", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var userCalendar = await TestAppDb.GetCollection<UserCalendarDocument>()
            .Find(x => x.Day == command.Day)
            .FirstOrDefaultAsync();

        userCalendar.ShouldNotBeNull();
        var eventDocument = userCalendar.Events.FirstOrDefault(x => x.Id == command.Id);
        eventDocument.ShouldBeOfType<ImportantDateDocument>();
        eventDocument.Title.ShouldBe(command.Title);
    }
    
    [Fact]
    public async Task AddImportantDate_GivenExistingUserCalendar_ShouldReturn201CreatedStatusCodeUpdateserCalendarWithImportantDateEvent()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var @event = MeetingFactory.GetInUserCalender(userCalendar);
        await TestAppDb.GetCollection<UserCalendarDocument>().InsertOneAsync(userCalendar.AsDocument());
        var command = new AddImportantDateCommand(userCalendar.Day, Guid.NewGuid(), "test_title");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-important-date", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var updatedUserCalendar = await TestAppDb.GetCollection<UserCalendarDocument>()
            .Find(x => x.Day == command.Day)
            .FirstOrDefaultAsync();
        var eventDocument = updatedUserCalendar.Events.FirstOrDefault(x => x.Id == command.Id);
        eventDocument.ShouldBeOfType<ImportantDateDocument>();
        eventDocument.Title.ShouldBe(command.Title);
    }
    
    [Fact]
    public async Task AddImportantDate_GivenEmptyTitle_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        var command = new AddImportantDateCommand(DateOnly.FromDateTime(DateTime.Now), Guid.NewGuid(), string.Empty);
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-important-date", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}