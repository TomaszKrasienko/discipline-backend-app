using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.tests.sharedkernel.Application;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.DTOs;

public sealed class UpdateActivityRuleDtoMapperExtensionsTests
{
    [Fact]
    public void MapAsCommand_GivenUpdateActivityRuleDtoWithActivityRuleId_ShouldReturnUpdateActivityRuleId()
    {
        //arrange
        var dto = UpdateActivityRuleDtoFakeDataFactory.Get();
        var id = ActivityRuleId.New();
        
        //act
        var result = dto.MapAsCommand(id);
        
        //assert
        result.Id.ShouldBe(id);
        result.Title.ShouldBe(dto.Title);
        result.Mode.ShouldBe(dto.Mode);
        result.SelectedDays.ShouldBeEquivalentTo(dto.SelectedDays);
    }
}