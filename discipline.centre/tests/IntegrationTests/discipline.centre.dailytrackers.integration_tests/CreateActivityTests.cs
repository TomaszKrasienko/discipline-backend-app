using discipline.centre.dailytrackers.application.DailyTrackers.DTOs;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.integration_tests.shared;
using Xunit;

namespace discipline.centre.dailytrackers.integration_tests;

public sealed class CreateActivityTests() : BaseTestsController("daily-trackers-module")
{
    [Fact]
    public async Task GivenValidArguments_ShouldReturn200OkStatusCodeAndAddActivity()
    {
        //arrange 
        var user = await AuthorizeWithoutSubscription();

        var createActivityDto = new CreateActivityDto(DateOnly.FromDateTime(DateTime.Now),
            new ActivityDetailsSpecification("test_activity_title", "test_activity_note"),
            [new StageSpecification("test_stage_title", 1)]);
        
        //act
        var response = await HttpClient.PostAsync("api/daily-trackers-module/daily-trackers/activities")
    }
}