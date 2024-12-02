using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain.unit_tests.StageTests;

public partial class CreateTests
{
    public static IEnumerable<object?[]> GetInvalidCreateStageData()
    {
        yield return 
        [
            new CreateStageParams(StageId.New(), string.Empty, 1),
            "ActivityRule.Stage.Title.Empty"
        ];
        
        yield return 
        [
            new CreateStageParams(StageId.New(), new string('t', 31), 1),
            "ActivityRule.Stage.Title.TooLong"
        ];
        
        yield return 
        [
            new CreateStageParams(StageId.New(), "test_title", -1), 
            "ActivityRule.Stage.Index.LessThanOne"
        ];
    }
}

public sealed record CreateStageParams(StageId StageId, string Title, int Index);