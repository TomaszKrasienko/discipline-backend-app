using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Features.DailyProductivities;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.Users;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Entities;
using discipline.tests.shared.Entities;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.DailyProductivities;

[Collection("integration-tests")]
public sealed class CreateActivityTests : BaseTestsController
{
    [Fact]
    public async Task Create_GivenForFirstDailyActivity_ShouldReturn200OkStatusCodeAndAddDailyProductivityWithActivity()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var day = DateTime.Now;
        var command = new CreateActivityCommand(new ActivityId(Ulid.Empty), user.Id, "Test title", default);
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"/daily-productivity/{day:yyyy-MM-dd}/add-activity", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var dailyProductivityDocument = await TestAppDb
            .GetCollection<DailyProductivityDocument>()
            .Find(x => x.Day == DateOnly.FromDateTime(DateTime.Now.Date) && x.UserId == user.Id.Value)
            .FirstOrDefaultAsync();

        dailyProductivityDocument.ShouldNotBeNull();
        dailyProductivityDocument.Activities.Any(x => x.Title == command.Title).ShouldBeTrue();
    }
    
    [Fact]
    public async Task Create_GivenForExistingDailyActivity_ShouldReturn200OkStatusCodeAndAddActivity()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var dailyProductivity = DailyProductivityFactory.Get();
        var activity = ActivityFactory.GetInDailyProductivity(dailyProductivity);
        await TestAppDb.GetCollection<DailyProductivityDocument>()
            .InsertOneAsync(dailyProductivity.AsDocument());
        var command = new CreateActivityCommand(new ActivityId(Ulid.Empty), user.Id, "Test title", dailyProductivity.Day);
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"/daily-productivity/{dailyProductivity.Day.Value:yyyy-MM-dd}/add-activity", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
   
        var dailyProductivityDocument = await TestAppDb
            .GetCollection<DailyProductivityDocument>()
            .Find(x => x.Day == DateOnly.FromDateTime(DateTime.Now.Date))
            .FirstOrDefaultAsync();

        dailyProductivityDocument.ShouldNotBeNull();
        dailyProductivityDocument.Activities.Any(x => x.Title == command.Title).ShouldBeTrue();
    }
    
    [Fact]
    public async Task Create_GivenInvalidActivity_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var dailyProductivity = DailyProductivityFactory.Get();
        var activity = ActivityFactory.GetInDailyProductivity(dailyProductivity);
        await TestAppDb.GetCollection<DailyProductivityDocument>()
            .InsertOneAsync(dailyProductivity.AsDocument());
        var command = new CreateActivityCommand(new ActivityId(Ulid.Empty), user.Id, activity.Title, dailyProductivity.Day);
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"/daily-productivity/{dailyProductivity.Day.Value:yyyy-MM-dd}/add-activity", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_Unauthorized_ShouldReturn401UnauthorizedSStatusCode()
    {
        //arrange
        var command = new CreateActivityCommand(new ActivityId(Ulid.Empty), new UserId(Ulid.Empty), "test_title", new DateOnly(2024,1,1));
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"/daily-productivity/{command.Day:yyyy-MM-dd}/add-activity", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Create_AuthorizedByUserWithStatusCreated_ShouldReturnResponse403ForbiddenStatusCode()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        var command = new CreateActivityCommand(new ActivityId(Ulid.Empty), new UserId(Ulid.Empty), "test_title", new DateOnly(2024,1,1));
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"/daily-productivity/{command.Day:yyyy-MM-dd}/add-activity", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Create_GivenEmptyTitle_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var day = DateTime.Now;
        var command = new CreateActivityCommand(new ActivityId(Ulid.Empty), user.Id, string.Empty, default);
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"/daily-productivity/{day:yyyy-MM-dd}/add-activity", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}