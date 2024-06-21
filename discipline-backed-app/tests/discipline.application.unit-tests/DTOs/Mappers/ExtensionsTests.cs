using discipline.application.DTOs.Mappers;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.DTOs.Mappers;

public sealed class ExtensionsTests
{
    [Fact]
    public void AsDto_GivenActivityRuleWithSelectedDays_ShouldReturnActivityRuleDto()
    {
        //arrange
        List<int> selectedDays = [1, 3, 5];
        var entity = ActivityRuleFactory.Get(selectedDays);
        
        //act
        var result = entity.AsDto();
        
        //assert
        result.Id.ShouldBe(entity.Id.Value);
        result.Title.ShouldBe(entity.Title.Value);
        result.Mode.ShouldBe(entity.Mode.Value);
        result.SelectedDays[0].ShouldBe(entity.SelectedDays[0].Value);
        result.SelectedDays[1].ShouldBe(entity.SelectedDays[1].Value);
        result.SelectedDays[2].ShouldBe(entity.SelectedDays[2].Value);
    }
    
    
    [Fact]
    public void AsDto_GivenActivityRuleWithoutSelectedDays_ShouldReturnActivityRuleDto()
    {
        //arrange
        var entity = ActivityRuleFactory.Get();
        
        //act
        var result = entity.AsDto();
        
        //assert
        result.Id.ShouldBe(entity.Id.Value);
        result.Title.ShouldBe(entity.Title.Value);
        result.Mode.ShouldBe(entity.Mode.Value);
        result.SelectedDays.ShouldBeNull();
    }

    [Fact]
    public void AsDto_GivenActivity_ShouldReturnActivityDto()
    {
        //arrange
        var activity = ActivityFactory.Get();
        
        //act
        var result = activity.AsDto();
        
        //assert
        result.Id.ShouldBe(activity.Id.Value);
        result.Title.ShouldBe(activity.Title.Value);
        result.IsChecked.ShouldBe(activity.IsChecked.Value);
        result.ParentRuleId.ShouldBeNull();
    }
}