using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.DailyTrackerTests;

public sealed class AddActivityTests
{
    [Fact]
    public void GivenUniqueTitle_ShouldAddActivity()
    {
        //arrange
        var title = "test_activity_title";
        var note = "test_activity_note";
        var parentActivityRuleId = ActivityRuleId.New();
        var stages = new List<StageSpecification>
        {
            new ("test_state_title", 1)
        };
        
        var dailyTracker = DailyTracker.Create(DailyTrackerId.New(), DateOnly.FromDateTime(DateTime.UtcNow), UserId.New(),
            new ActivityDetailsSpecification("test_title", null), null, null);
        
        //act
        var activity = dailyTracker.AddActivity(ActivityId.New(), new ActivityDetailsSpecification(title, note),
            parentActivityRuleId, stages);

        //assert
        var newActivity = dailyTracker.Activities.Single(x => x.Id == activity.Id);
        newActivity.Details.Title.ShouldBe(title);
        newActivity.Details.Note.ShouldBe(note);
        newActivity.ParentActivityRuleId.ShouldBe(parentActivityRuleId);
        newActivity.Stages![0].Title.Value.ShouldBe(stages[0].Title);
        newActivity.Stages![0].Index.Value.ShouldBe(stages[0].Index);
    }
    
    [Fact]
    public void GivenAlreadyExistingTitle_ShouldThrowDomainExceptionWithCodeDailyTrackerActivityTitleAlreadyExists()
    {
        //arrange
        var title = "test_activity_title";
        var dailyTracker = DailyTracker.Create(DailyTrackerId.New(), DateOnly.FromDateTime(DateTime.UtcNow), UserId.New(),
            new ActivityDetailsSpecification(title, null), null, null);
        
        //act
        var exception = Record.Exception(() => dailyTracker.AddActivity(ActivityId.New(), new ActivityDetailsSpecification(title, null),
            null, null));

        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Activity.Title.AlreadyExists");
    }
}