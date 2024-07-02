using discipline.application.Features.Progress;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Features.Progress.GetProgressData;

public sealed class ProgressCalculatorTests
{
    [Theory]
    [InlineData(10, 6, 60)]
    [InlineData(11, 4, 36)]
    public void Calculate_GivenData_ShouldCalculatePercent(int activities, int doneActivities, int percent)
    {
        //act
        var result = ProgressCalculator.Calculate(activities, doneActivities);
        
        //assert
        result.ShouldBe(percent);
    }
}