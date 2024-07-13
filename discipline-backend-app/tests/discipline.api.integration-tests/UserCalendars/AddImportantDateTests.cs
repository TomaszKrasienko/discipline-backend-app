using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Features.UsersCalendars;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;
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
}