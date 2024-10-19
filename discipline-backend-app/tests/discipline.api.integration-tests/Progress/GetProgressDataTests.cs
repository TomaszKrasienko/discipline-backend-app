using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.DTOs;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Repositories;
using discipline.domain.DailyProductivities.Entities;
using discipline.domain.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.Progress;

[Collection("integration-tests")]
public sealed class GetProgressDataTests : BaseTestsController
{
    [Fact]
    public async Task GetProgressData_GivenFilledData_ShouldReturnIEnumerableOfProgressDataDto()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        var dailyProductivity1 = DailyProductivity.Create(DailyProductivityId.New(),  new DateOnly(2024, 6, 10), 
            UserId.New());
        dailyProductivity1.AddActivity(ActivityId.New(), "test 1");
        dailyProductivity1.AddActivity(ActivityId.New(), "test 2");
        dailyProductivity1.AddActivity(ActivityId.New(), "test 3");
        dailyProductivity1.ChangeActivityCheck(dailyProductivity1.Activities.First(x => x.Title == "test 1").Id);
        dailyProductivity1.ChangeActivityCheck(dailyProductivity1.Activities.First(x => x.Title == "test 2").Id);
        
        var dailyProductivity2 = DailyProductivity.Create(DailyProductivityId.New(), new DateOnly(2024, 6, 11),
            UserId.New());
        dailyProductivity2.AddActivity(ActivityId.New(), "test 1");
        dailyProductivity2.AddActivity(ActivityId.New(), "test 2");
        dailyProductivity2.AddActivity(ActivityId.New(), "test 3");
        dailyProductivity2.ChangeActivityCheck(dailyProductivity2.Activities.First(x => x.Title == "test 1").Id);
        dailyProductivity2.ChangeActivityCheck(dailyProductivity2.Activities.First(x => x.Title == "test 2").Id);
        dailyProductivity2.ChangeActivityCheck(dailyProductivity2.Activities.First(x => x.Title == "test 3").Id);
        
        var dailyProductivity3 = DailyProductivity.Create(DailyProductivityId.New(), new DateOnly(2024, 6, 12),
            UserId.New());

        var collection =
            TestAppDb.GetCollection<DailyProductivityDocument>();

        await collection.InsertManyAsync([dailyProductivity1.AsDocument(), dailyProductivity2.AsDocument(), dailyProductivity3.AsDocument()]);
        
        //act
        var results = await HttpClient.GetFromJsonAsync<IEnumerable<ProgressDataDto>>("progress/data");
        
        //assert
        var resultsList = results.ToList();
        resultsList.ToList()[0].Day.ShouldBe(dailyProductivity1.Day.Value);
        resultsList.ToList()[0].Percent.ShouldBe(67);
        resultsList.ToList()[1].Day.ShouldBe(dailyProductivity2.Day.Value);
        resultsList.ToList()[1].Percent.ShouldBe(100);
        resultsList.ToList()[2].Day.ShouldBe(dailyProductivity3.Day.Value);
        resultsList.ToList()[2].Percent.ShouldBe(0);
    }
    
    [Fact]
    public async Task GetProgressData_GivenEmptyData_ShouldReturnNoContentStatusCode()
    {
        //act
        await AuthorizeWithFreeSubscriptionPicked();
        var response = await HttpClient.GetAsync("progress/data");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task GetProgressData_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var response = await HttpClient.GetAsync("progress/data");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetProgressData_GivenAuthorizedWithoutPickedSubscription_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        
        //act
        var response = await HttpClient.GetAsync("progress/data");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
