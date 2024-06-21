using discipline.application.Domain.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Entities.DailyProductivities;

public sealed class DailyProductivityCreateTests
{
    [Fact]
    public void Create_GivenWithArguments_ShouldReturnDailyProductivityWithFilledFields()
    {
        //arrange
        var day = DateTime.Now;
        
        //act
        var result = DailyProductivity.Create(day);
        
        //assert
        result.Day.Value.ShouldBe(day.Date);
    } 
}