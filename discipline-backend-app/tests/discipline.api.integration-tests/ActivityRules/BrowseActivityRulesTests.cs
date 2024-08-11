using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.DTOs;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.Users;
using discipline.domain.Users.Entities;
using discipline.tests.shared.Documents;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.ActivityRules;

[Collection("integration-tests")]
public sealed class BrowseActivityRulesTests : BaseTestsController
{
    [Fact]
    public async Task BrowseActivityRules_GivenPaginationDataAndAuthorized_ShouldReturnDataForUser()
    {
        //arrange
        var user = await AuthorizeWithUser();
        var activityRules = ActivityRuleDocumentFactory.Get(5);
        activityRules.ForEach(x => x.UserId = user.Id);
        var notUserActivityRules = ActivityRuleDocumentFactory.Get(2);
        await TestAppDb.GetCollection<ActivityRuleDocument>()
            .InsertManyAsync(activityRules);
        await TestAppDb.GetCollection<ActivityRuleDocument>()
            .InsertManyAsync(notUserActivityRules);
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
    
    private async Task<User> AuthorizeWithUser()
    {
        var subscription = SubscriptionFactory.Get();
        var user = UserFactory.Get();
        user.CreateFreeSubscriptionOrder(Guid.NewGuid(), subscription, DateTime.Now);
        await TestAppDb.GetCollection<UserDocument>().InsertOneAsync(user.AsDocument());
        Authorize(user.Id, user.Status);
        return user;
    }
}