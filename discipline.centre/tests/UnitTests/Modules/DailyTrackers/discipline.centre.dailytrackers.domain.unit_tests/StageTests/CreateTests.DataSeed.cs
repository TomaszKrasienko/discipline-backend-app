using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.domain.unit_tests.StageTests;

public partial class CreateTests
{
    public static IEnumerable<object[]> GetInvalidCreateData()
    {
        yield return
        [
            new CreateTestParameters(StageId.New(), string.Empty, 1),
            "DailyTracker.Stage.Title.Empty"
        ];
        
        yield return
        [
            new CreateTestParameters(StageId.New(), new string('t', 31), 1),
            "DailyTracker.Stage.Title.TooLong"
        ];
        
        yield return
        [
            new CreateTestParameters(StageId.New(), "test_stage_title", 0),
            "DailyTracker.Stage.Index.LessThanOne"
        ];
    }

    public sealed record CreateTestParameters(StageId StageId, string Title, int Index);
}