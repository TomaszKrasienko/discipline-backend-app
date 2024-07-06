using discipline.application.Domain.Entities;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.tests.shared.Documents;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Infrastructure.DAL;

public sealed class DocumentsMappersExtensionsTests
{
    [Fact]
    public void AsDocument_GivenActivityRuleWithoutSelectedDays_ShouldReturnActivityRuleDocumentWithNullSelectedDays()
    {
        //arrange
        var activityRule = ActivityRuleFactory.Get();
        
        //act
        var result = activityRule.AsDocument();
        
        //assert
        result.Id.ShouldBe(activityRule.Id.Value);
        result.Title.ShouldBe(activityRule.Title.Value);
        result.Mode.ShouldBe(activityRule.Mode.Value);
        result.SelectedDays.ShouldBeNull();
    }
    
    [Fact]
    public void AsDocument_GivenActivityRuleWithSelectedDays_ShouldReturnActivityRuleDocument()
    {
        //arrange
        List<int> selectedDays = [0, 1, 2];
        var activityRule = ActivityRuleFactory.Get(selectedDays);
        
        //act
        var result = activityRule.AsDocument();
        
        //assert
        result.Id.ShouldBe(activityRule.Id.Value);
        result.Title.ShouldBe(activityRule.Title.Value);
        result.Mode.ShouldBe(activityRule.Mode.Value);
        result.SelectedDays.Contains(selectedDays[0]).ShouldBeTrue();
        result.SelectedDays.Contains(selectedDays[1]).ShouldBeTrue();
        result.SelectedDays.Contains(selectedDays[2]).ShouldBeTrue();
    }

    [Fact]
    public void AsEntity_GivenActivityRuleDocumentWithoutSelectedDays_ShouldReturnActivityRuleWithEmptySelectedDays()
    {
        //arrange
        var activityRuleDocument = ActivityRuleDocumentFactory.Get();
        
        //act
        var result = activityRuleDocument.AsEntity();
        
        //assert
        result.Id.Value.ShouldBe(activityRuleDocument.Id);
        result.Title.Value.ShouldBe(activityRuleDocument.Title);
        result.Mode.Value.ShouldBe(activityRuleDocument.Mode);
        result.SelectedDays.ShouldBeEmpty();
    }
    
    [Fact]
    public void AsEntity_GivenActivityRuleDocumentWithSelectedDays_ShouldReturnActivityRule()
    {
        //arrange
        List<int> selectedDays = [0, 1, 2];
        var activityRuleDocument = ActivityRuleDocumentFactory.Get(selectedDays);
        
        //act
        var result = activityRuleDocument.AsEntity();
        
        //assert
        result.Id.Value.ShouldBe(activityRuleDocument.Id);
        result.Title.Value.ShouldBe(activityRuleDocument.Title);
        result.Mode.Value.ShouldBe(activityRuleDocument.Mode);
        result.SelectedDays.Select(x => x.Value).ToList().Contains(selectedDays[0]).ShouldBeTrue();
        result.SelectedDays.Select(x => x.Value).ToList().Contains(selectedDays[1]).ShouldBeTrue();
        result.SelectedDays.Select(x => x.Value).ToList().Contains(selectedDays[2]).ShouldBeTrue();
    }

    [Fact]
    public void AsDto_GivenActivityRuleDocumentWithoutSelectedDays_ShouldReturnActivityRuleDtoWithSelectedDaysAsNull()
    {
        //arrange
        var activityRuleDocument = ActivityRuleDocumentFactory.Get();
        
        //act
        var result = activityRuleDocument.AsDto();
        
        //assert
        result.Id.ShouldBe(activityRuleDocument.Id);
        result.Title.ShouldBe(activityRuleDocument.Title);
        result.Mode.ShouldBe(activityRuleDocument.Mode);
        result.SelectedDays.ShouldBeNull();
    }
    
    [Fact]
    public void AsDto_GivenActivityRuleDocumentWithSelectedDays_ShouldReturnActivityRuleDto()
    {
        //arrange
        List<int> selectedDays = [1, 4];
        var activityRuleDocument = ActivityRuleDocumentFactory.Get(selectedDays);
        
        //act
        var result = activityRuleDocument.AsDto();
        
        //assert
        result.Id.ShouldBe(activityRuleDocument.Id);
        result.Title.ShouldBe(activityRuleDocument.Title);
        result.Mode.ShouldBe(activityRuleDocument.Mode);
        result.SelectedDays.Contains(selectedDays[0]).ShouldBeTrue();
        result.SelectedDays.Contains(selectedDays[1]).ShouldBeTrue();
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
    
}