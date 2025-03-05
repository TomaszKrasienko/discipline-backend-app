using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.domain.unit_tests.DailyTrackerTests;

public partial class CreateTests
{
    public static IEnumerable<object[]> GetValidCreateData()
    {
        yield return
        [
            new CreateTestParameters(DailyTrackerId.New(), DateOnly.FromDateTime(DateTime.UtcNow), UserId.New(),
                new ActivityDetailsSpecification("test_title_activity", null), null, null)
        ];
    }
    
    public static IEnumerable<object[]> GetInvalidCreateData()
    {
        yield return
        [
            new CreateTestParameters(DailyTrackerId.New(), default, UserId.New(),
                new ActivityDetailsSpecification("test_title_activity", null), null, null),
            "DailyTracker.Day.Default"
        ];
    }
    
    public sealed record CreateTestParameters(DailyTrackerId DailyTrackerId, DateOnly Day, UserId UserId, ActivityDetailsSpecification Details,
        ActivityRuleId? ParentActivityRuleId, List<StageSpecification>? Stages);
}