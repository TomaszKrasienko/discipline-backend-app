using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Domain.ActivityRules.Entities;
using discipline.application.Domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.application.Features.DailyProductivities;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Repositories;
using discipline.tests.shared.Entities;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.DailyProductivities;

[Collection("integration-tests")]
public sealed class CreateActivityFromRuleTests : BaseTestsController
{
    [Fact]
    public async Task CreateActivityFromRule_GivenExistingActivityRule_ShouldReturn200OkStatusCodeAndAddActivity()
    {
        //arrange
        var activityRule = ActivityRule.Create(Guid.NewGuid(), "test_title", Mode.EveryDayMode());
        await TestAppDb.GetCollection<ActivityRuleDocument>("ActivityRules").InsertOneAsync(activityRule.AsDocument());
        var command = new CreateActivityFromRuleCommand(Guid.Empty, activityRule.Id);
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/daily-productivity/today/add-activity-from-rule", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var isExists = await TestAppDb
            .GetCollection<DailyProductivityDocument>(MongoDailyProductivityRepository.CollectionName)
            .Find(x 
                => x.Activities.Any(a 
                    => a.ParentRuleId.Equals(command.ActivityRuleId)
                    && a.Title == activityRule.Title.Value))
            .AnyAsync();
        isExists.ShouldBeTrue();
    }
    
    [Fact]
    public async Task CreateActivityFromRule_GivenNotExistingActivityRule_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var command = new CreateActivityFromRuleCommand(Guid.Empty, Guid.NewGuid());
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/daily-productivity/today/add-activity-from-rule", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateActivityFromRule_GivenInvalidCommand_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        var command = new CreateActivityFromRuleCommand(Guid.Empty, Guid.Empty);
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/daily-productivity/today/add-activity-from-rule", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}