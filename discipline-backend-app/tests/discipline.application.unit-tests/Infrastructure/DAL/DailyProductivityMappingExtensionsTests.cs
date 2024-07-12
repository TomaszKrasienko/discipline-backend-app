using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.tests.shared.Documents;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Infrastructure.DAL;

public sealed class DailyProductivityMappingExtensionsTests
{
    [Fact]
    public void AsDocument_GivenDailyProductivityWithoutActivities_ShouldReturnDailyProductivityDocumentWithoutActivities()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        
        //act
        var result = dailyProductivity.AsDocument();
        
        //assert
        result.Day.ShouldBe(dailyProductivity.Day.Value);
    }
    
    [Fact]
    public void AsDocument_GivenDailyProductivityWithActivities_ShouldReturnDailyProductivityDocumentWithActivities()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var activity = ActivityFactory.GetInDailyProductivity(dailyProductivity);
        
        //act
        var result = dailyProductivity.AsDocument();
        
        //assert
        result.Day.ShouldBe(dailyProductivity.Day.Value);
        result.Activities.Any(x
            => x.Id.Equals(activity.Id)
               && x.Title == activity.Title
               && x.IsChecked == activity.IsChecked).ShouldBeTrue();
    }
    
    [Fact]
    public void AsEntity_GivenDailyProductivityDocumentWithoutActivities_ShouldReturnDailyProductivityWithoutActivities()
    {
        //arrange
        var dailyProductivityDocument = DailyProductivityDocumentFactory.Get();
        
        //act
        var result = dailyProductivityDocument.AsEntity();
        
        //assert
        result.Day.Value.ShouldBe(dailyProductivityDocument.Day);
    }
    
    [Fact]
    public void AsEntity_GivenDailyProductivityDocumentWithActivities_ShouldReturnDailyProductivityWithActivities()
    {
        //arrange
        var activityDocuments = ActivityDocumentFactory.Get(1);
        var dailyProductivityDocument = DailyProductivityDocumentFactory.Get(activityDocuments);
        
        //act
        var result = dailyProductivityDocument.AsEntity();
        
        //assert
        result.Day.Value.ShouldBe(dailyProductivityDocument.Day);
        result.Activities.Any(x
            => x.Id.Value.Equals(activityDocuments[0].Id)
               && x.Title == activityDocuments[0].Title
               && x.IsChecked == activityDocuments[0].IsChecked).ShouldBeTrue();
    }
    
    [Fact]
    public void AsDto_GivenDailyProductivityDocumentWithoutActivities_ShouldReturnDailyProductivityDtoWithoutActivitiesDto()
    {
        //arrange
        var dailyProductivityDocument = DailyProductivityDocumentFactory.Get();
        
        //act
        var result = dailyProductivityDocument.AsDto();
        
        //assert
        result.Day.ShouldBe(dailyProductivityDocument.Day);
    }
    
    [Fact]
    public void AsDto_GivenDailyProductivityDocumentWithActivities_ShouldReturnDailyProductivityDtoWithActivitiesDto()
    {
        //arrange
        var activityDocuments = ActivityDocumentFactory.Get(1);
        var dailyProductivityDocument = DailyProductivityDocumentFactory.Get(activityDocuments);
        
        //act
        var result = dailyProductivityDocument.AsDto();
        
        //assert
        result.Day.ShouldBe(dailyProductivityDocument.Day);
        result.Activities.Any(x
            => x.Id.Equals(activityDocuments[0].Id)
               && x.Title == activityDocuments[0].Title
               && x.IsChecked == activityDocuments[0].IsChecked).ShouldBeTrue();
    }
    
    [Fact]
    public void AsDocument_GivenActivity_ShouldReturnActivityDocument()
    {
        //arrange
        var activity = ActivityFactory.Get();
        
        //act
        var result = activity.AsDocument();
        
        //assert
        result.Id.ShouldBe(activity.Id.Value);
        result.Title.ShouldBe(activity.Title.Value);
        result.IsChecked.ShouldBe(activity.IsChecked.Value);
        result.ParentRuleId.ShouldBeNull();
    }

    [Fact]
    public void AsEntity_GivenActivityDocument_ShouldReturnActivity()
    {
        //arrange
        var activityDocument = ActivityDocumentFactory.Get();
        
        //act
        var result = activityDocument.AsEntity();
        
        //assert
        result.Id.Value.ShouldBe(activityDocument.Id);
        result.Title.Value.ShouldBe(activityDocument.Title);
        result.IsChecked.Value.ShouldBe(activityDocument.IsChecked);
        result.ParentRuleId.ShouldBeNull();
    }
}