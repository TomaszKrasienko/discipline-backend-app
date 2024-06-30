using discipline.application.Behaviours;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Behaviours;

public sealed class ConvertingToQuartzExpressionBehaviourTests
{
    [Theory]
    [InlineData(12,12,12, "12 12 12 * * ?")]    
    [InlineData(0,11,10, "* 11 10 * * ?")]
    public void AsQuartzExpression_GivenTime_ShouldReturnStringAsQuartzExpression(int seconds, int minutes, int hours, string expression)
    {
        //arrange
        var time = new TimeOnly(hours, minutes, seconds);
        
        //act 
        var result = time.AsQuartzExpression();
        
        //assert
        result.ShouldBe(expression);
    }
}