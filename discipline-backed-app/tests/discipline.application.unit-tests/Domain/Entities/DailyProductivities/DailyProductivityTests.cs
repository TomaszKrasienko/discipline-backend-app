using discipline.application.Domain.Exceptions;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Entities.DailyProductivities;

public sealed class DailyProductivityTests
{
    [Fact]
    public void AddActivity_GivenNotExistedTitle_ShouldAddToActivities()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var id = Guid.NewGuid();
        var title = "title";
        //act
        dailyProductivity.AddActivity(id, title);
        
        //assert
        dailyProductivity
            .Activities
            .Any(x => x.Id.Equals(id) && x.Title == title)
            .ShouldBeTrue();
    }
    
    [Fact]
    public void AddActivity_GivenAlreadyExistingTitle_ShouldThrowActivityTitleAlreadyRegisteredException()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var title = "Activity title";
        dailyProductivity.AddActivity(Guid.NewGuid(), title);
        
        //act
        var exception = Record.Exception(() => dailyProductivity.AddActivity(Guid.NewGuid(), title));
        
        //assert
        exception.ShouldBeOfType<ActivityTitleAlreadyRegisteredException>();
    }
}