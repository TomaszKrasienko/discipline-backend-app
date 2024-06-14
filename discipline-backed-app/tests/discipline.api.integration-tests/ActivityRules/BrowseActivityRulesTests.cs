using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.DTOs;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.ActivityRules;

public sealed class BrowseActivityRulesTests : BaseTestsController
{
    [Fact]
    public async Task BrowseActivityRules_GivenPaginationData_ShouldReturnData()
    {
        //arrange
        var activityRules = ActivityRuleFactory.Get(5);
        await DbContext.ActivityRules.AddRangeAsync(activityRules);
        await DbContext.SaveChangesAsync();
        var pageNumber = 1;
        var pageSize = 3;
        
        //act
        var response = await HttpClient.GetAsync($"/activity-rules?pageNumber={pageNumber}&pageSize={pageSize}");
        
        //assert
        var result = await response.Content.ReadFromJsonAsync<List<ActivityRuleDto>>();
        result.Count.ShouldBe(pageSize);

        var metaData = GetMetaDataFromHeader(response);
        metaData.CurrentPage.ShouldBe(pageNumber);
        metaData.TotalPages.ShouldBe(2);
        metaData.HasPrevious.ShouldBeFalse();
        metaData.HasNext.ShouldBeTrue();
        metaData.TotalCount.ShouldBe(5);
        metaData.PageSize.ShouldBe(pageSize);
    }
}