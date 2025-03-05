using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.DailyTrackerTests;

public partial class CreateTests
{
    [Theory]
    [MemberData(nameof(GetValidCreateData))] 
    public void GivenValidArguments_ShouldReturnDailyTrackerWithValue(CreateTestParameters parameters)
    {
        //act
        var result = DailyTracker.Create(parameters.DailyTrackerId, parameters.Day, parameters.UserId, ActivityId.New(), 
            parameters.Details, parameters.ParentActivityRuleId, parameters.Stages);
        
        //assert
        result.Id.ShouldBe(parameters.DailyTrackerId);
        result.Day.Value.ShouldBe(parameters.Day);
        result.UserId.ShouldBe(parameters.UserId);
        result.Activities.First().Details.Title.ShouldBe(parameters.Details.Title);
        result.Activities.First().Details.Note.ShouldBe(parameters.Details.Note);
        result.Activities.First().ParentActivityRuleId.ShouldBe(parameters.ParentActivityRuleId);
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidCreateData))]
    public void GivenInvalidArguments_ShouldThrowDomainExceptionWithCode(CreateTestParameters parameters, string code)
    {
        //act
        var exception = Record.Exception(() => DailyTracker.Create(parameters.DailyTrackerId, parameters.Day, parameters.UserId, ActivityId.New(),
            parameters.Details, parameters.ParentActivityRuleId, parameters.Stages));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe(code);
    }
}