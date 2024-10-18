using discipline.domain.DailyProductivities.Entities;
using discipline.domain.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.DailyProductivities.Entities.Create;

public sealed class DailyProductivityCreateTests
{
    [Fact]
    public void Create_GivenWithArguments_ShouldReturnDailyProductivityWithFilledFields()
    {
        //arrange
        var id = DailyProductivityId.New();
        var day = DateTime.Now;
        var userId = UserId.New();
        
        //act
        var result = DailyProductivity.Create(id, DateOnly.FromDateTime(day), userId);
        
        //assert
        result.Id.ShouldBe(id);
        result.Day.Value.ShouldBe(DateOnly.FromDateTime(day));
        result.UserId.ShouldBe(userId);
    } 
}