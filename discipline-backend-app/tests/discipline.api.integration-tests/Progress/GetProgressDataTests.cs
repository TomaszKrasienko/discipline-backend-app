using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Domain.DailyProductivities.Entities;
using discipline.application.DTOs;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Repositories;
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
        var dailyProductivity1 = DailyProductivity.Create(new DateOnly(2024, 6, 10));
        dailyProductivity1.AddActivity(Guid.NewGuid(), "test 1");
        dailyProductivity1.AddActivity(Guid.NewGuid(), "test 2");
        dailyProductivity1.AddActivity(Guid.NewGuid(), "test 3");
        dailyProductivity1.ChangeActivityCheck(dailyProductivity1.Activities.First(x => x.Title == "test 1").Id);
        dailyProductivity1.ChangeActivityCheck(dailyProductivity1.Activities.First(x => x.Title == "test 2").Id);
        
        var dailyProductivity2 = DailyProductivity.Create(new DateOnly(2024, 6, 11));
        dailyProductivity2.AddActivity(Guid.NewGuid(), "test 1");
        dailyProductivity2.AddActivity(Guid.NewGuid(), "test 2");
        dailyProductivity2.AddActivity(Guid.NewGuid(), "test 3");
        dailyProductivity2.ChangeActivityCheck(dailyProductivity2.Activities.First(x => x.Title == "test 1").Id);
        dailyProductivity2.ChangeActivityCheck(dailyProductivity2.Activities.First(x => x.Title == "test 2").Id);
        dailyProductivity2.ChangeActivityCheck(dailyProductivity2.Activities.First(x => x.Title == "test 3").Id);

        var collection =
            TestAppDb.GetCollection<DailyProductivityDocument>();

        await collection.InsertManyAsync([dailyProductivity1.AsDocument(), dailyProductivity2.AsDocument()]);
        
        //act
        var results = await HttpClient.GetFromJsonAsync<IEnumerable<ProgressDataDto>>("progress/data");
        
        //assert
        var resultsList = results.ToList();
        resultsList.ToList()[0].Day.ShouldBe(dailyProductivity1.Day.Value);
        resultsList.ToList()[0].Percent.ShouldBe(67);
        resultsList.ToList()[1].Day.ShouldBe(dailyProductivity2.Day.Value);
        resultsList.ToList()[1].Percent.ShouldBe(100);
    }
    
    [Fact]
    public async Task GetProgressData_GivenEmptyData_ShouldReturnNoContentStatusCode()
    {
        //act
        var response = await HttpClient.GetAsync("progress/data");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}
