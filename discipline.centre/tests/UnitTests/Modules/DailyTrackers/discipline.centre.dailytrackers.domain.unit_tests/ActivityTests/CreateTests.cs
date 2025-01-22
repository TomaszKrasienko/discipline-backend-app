using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.ActivityTests;

public partial class CreateTests
{
    [Theory]
    [MemberData(nameof(GetValidCreateData))]
    public void GivenValidArguments_ShouldReturnActivityWithValues(CreateTestParameters parameters)
    {
        //act
        var result = Activity.Create(parameters.ActivityId, parameters.Details, parameters.ActivityRuleId,
            parameters.Stages);
        
        //assert
        result.Id.ShouldBe(parameters.ActivityId);
        result.Details.Title.ShouldBe(parameters.Details.Title);
        result.Details.Note.ShouldBe(parameters.Details.Note);
        result.ParentActivityRuleId.ShouldBe(parameters.ActivityRuleId);
        result.Stages.ShouldBeNull();
    }

    [Fact]
    public void GivenValidArgumentsWithStage_ShouldReturnActivityWithStage()
    {
        //arrange
        var stageTitle = "test_stage_title";
        var index = 1;
        
        //act
        var result = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_title", null),
            null, [new StageSpecification(stageTitle, index)]);
        
        //assert
        result.Stages![0].Title.Value.ShouldBe(stageTitle);
        result.Stages![0].Index.Value.ShouldBe(index);
    }

    [Theory]
    [MemberData(nameof(GetInvalidCreateData))]
    public void GivenInvalidData_ShouldThrowDomainExceptionWithCode(CreateTestParameters parameters, string code)
    {
        //act
        var exception = Record.Exception(() => Activity.Create(parameters.ActivityId, parameters.Details, parameters.ActivityRuleId,
            parameters.Stages));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe(code);
    }

    [Fact]
    public void GivenInvalidStageIndex_ShouldThrowDomainExceptionWithCode()
    {
        //act
        var exception = Record.Exception(() => Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_title", null),
             null, [new StageSpecification("test_stage_title", 2)]));
        
        //assert 
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Stages.MustHaveOrderedIndex");
    }
}

public partial class CreateTests
{
    public static IEnumerable<object[]> GetValidCreateData()
    {
        yield return
        [
            new CreateTestParameters(ActivityId.New(), new ActivityDetailsSpecification(
                "test_activity_title", null), null, null)
        ];

        yield return
        [
            new CreateTestParameters(ActivityId.New(), new ActivityDetailsSpecification(
                "test_activity_title", "test_activity_note"), ActivityRuleId.New(), null)
        ];
    }

    public static IEnumerable<object[]> GetInvalidCreateData()
    {
        yield return 
        [
            new CreateTestParameters(ActivityId.New(), new ActivityDetailsSpecification(
                string.Empty, null), null, null),
            "DailyTracker.Activity.Details.Title.Empty"
        ];
        
        yield return 
        [
            new CreateTestParameters(ActivityId.New(), new ActivityDetailsSpecification(
                new string('t', 31), null), null, null),
            "DailyTracker.Activity.Details.Title.TooLong"
        ];
    }

    public sealed record CreateTestParameters(
        ActivityId ActivityId,
        ActivityDetailsSpecification Details,
        ActivityRuleId? ActivityRuleId,
        List<StageSpecification>? Stages);
}