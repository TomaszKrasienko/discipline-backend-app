using System.Collections;

namespace discipline.centre.activityrules.domain.unit_tests.ValueObjects;

public partial class SelectedDaysTests
{
    public static IEnumerable<object[]> GetSelectedDaysValidData()
    {
        yield return
        [
            new List<int> { 0, 1, 2, 3, 4, 5, 6 }
        ];
    }

    public static IEnumerable<object[]> GetSelectedDaysInvalidData()
    {
        yield return
        [
            new List<int>{-1}
        ];
        
        yield return
        [
            new List<int>{0}
        ];
    }
}