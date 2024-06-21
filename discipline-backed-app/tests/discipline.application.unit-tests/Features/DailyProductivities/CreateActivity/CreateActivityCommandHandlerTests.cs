using discipline.application.Behaviours;
using discipline.application.Domain.Entities;
using discipline.application.Domain.Repositories;
using discipline.application.Features.Configuration.Base.Abstractions;
using discipline.application.Features.DailyProductivities;
using discipline.tests.shared.Entities;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Features.DailyProductivities.CreateActivity;

public sealed class CreateActivityCommandHandlerTests
{
    private Task Act(CreateActivityCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task HandleAsync_GivenFirstAtDay_ShouldCreateNewDailyProductivityAddAndUpdateByRepository()
    {
        //arrange
        var command = new CreateActivityCommand(Guid.NewGuid(), "New activity");
        var nowDate = DateTime.Now.Date; 
        
        _clock
            .DateNow()
            .Returns(nowDate);
        
        //act
        await Act(command);
        
        //assert
        await _dailyProductivityRepository
            .Received(1)
            .AddAsync(Arg.Is<DailyProductivity>(arg
                => arg.Day == nowDate
                && arg.Activities.Any(x => x.Id.Equals(command.Id) && x.Title == command.Title)));

        await _dailyProductivityRepository
            .Received(0)
            .UpdateAsync(Arg.Any<DailyProductivity>());
    }

    [Fact]
    public async Task HandleAsync_GivenForExistingDailyProductivity_ShouldUpdateDailyProductivityAndUpdateByRepository()
    {
        //arrange
        var nowDate = DateTime.Now.Date;
        var dailyActivity = DailyProductivityFactory.Get();
        var command = new CreateActivityCommand(Guid.NewGuid(), "New activity");

        _dailyProductivityRepository
            .GetByDateAsync(nowDate)
            .Returns(dailyActivity);
        
        _clock
            .DateNow()
            .Returns(nowDate);
        
        //act
        await Act(command);
        
        //assert
        await _dailyProductivityRepository
            .Received(1)
            .UpdateAsync(dailyActivity);

        await _dailyProductivityRepository
            .Received(0)
            .AddAsync(Arg.Any<DailyProductivity>());

        dailyActivity
            .Activities
            .Any(x => x.Id.Equals(command.Id) && x.Title == command.Title)
            .ShouldBeTrue();
    }
    
    #region arrange
    private readonly IDailyProductivityRepository _dailyProductivityRepository;
    private readonly IClock _clock;
    private readonly ICommandHandler<CreateActivityCommand> _handler;

    public CreateActivityCommandHandlerTests()
    {
        _dailyProductivityRepository = Substitute.For<IDailyProductivityRepository>();
        _clock = Substitute.For<IClock>(); 
        _handler = new CreateActivityCommandHandler(_dailyProductivityRepository, _clock);
    }
    #endregion
}