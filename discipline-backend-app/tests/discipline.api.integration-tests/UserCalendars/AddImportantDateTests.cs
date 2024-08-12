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
public sealed class AddImportantDateTests : BaseTestsController
{
    [Fact]
    public async Task AddImportantDate_GivenNotExistingUserCalendar_ShouldReturn201CreatedStatusCodeAddUserCalendarWithImportantDateEvent()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked(); 
        var command = new AddImportantDateCommand(DateOnly.FromDateTime(DateTime.Now), Guid.Empty, Guid.Empty, "test_title");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-important-date", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var userCalendar = await TestAppDb.GetCollection<UserCalendarDocument>()
            .Find(x => x.Day == command.Day && x.UserId == user.Id)
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
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var userCalendar = UserCalendarFactory.Get();
        var @event = MeetingFactory.GetInUserCalender(userCalendar);
        var userCalendarDocument = userCalendar.AsDocument();
        userCalendarDocument.UserId = user.Id;
        await TestAppDb.GetCollection<UserCalendarDocument>().InsertOneAsync(userCalendarDocument);
        var command = new AddImportantDateCommand(userCalendar.Day, Guid.Empty, Guid.Empty, "test_title");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-important-date", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);
        
        var updatedUserCalendar = await TestAppDb.GetCollection<UserCalendarDocument>()
            .Find(x => x.Day == command.Day && x.UserId == user.Id)
            .FirstOrDefaultAsync();
        var eventDocument = updatedUserCalendar.Events.FirstOrDefault(x => x.Id == resourceId);
        eventDocument.ShouldBeOfType<ImportantDateDocument>();
        eventDocument.Title.ShouldBe(command.Title);
    }
    
    [Fact]
    public async Task AddImportantDate_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new AddImportantDateCommand(DateOnly.FromDateTime(DateTime.Now), Guid.Empty, Guid.Empty, "test_title");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-calendar-event", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task AddImportantDate_GivenAuthorizedWithoutPickedSubscription_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        var command = new AddImportantDateCommand(DateOnly.FromDateTime(DateTime.Now), Guid.Empty, Guid.Empty, "test_title");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-calendar-event", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task AddImportantDate_GivenEmptyTitle_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        var command = new AddImportantDateCommand(DateOnly.FromDateTime(DateTime.Now), Guid.Empty, Guid.Empty, string.Empty);
        
        //act
        var response = await HttpClient.PostAsJsonAsync("user-calendar/add-important-date", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}