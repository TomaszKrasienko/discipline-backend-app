using discipline.application.Behaviours;
using discipline.application.Domain.DailyProductivities.Entities;
using discipline.application.Domain.DailyProductivities.Repositories;
using discipline.application.Features.DailyProductivities;
using discipline.tests.shared.Entities;
using NSubstitute;
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
        var nowDate = DateOnly.FromDateTime(DateTime.Now.Date);
        var command = new CreateActivityCommand(Guid.NewGuid(), Guid.NewGuid(),"New activity", nowDate);

        await _dailyProductivityRepository
            .GetByDateAsync(nowDate, default);
        
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
        var dailyProductivity = DailyProductivityFactory.Get();
        var command = new CreateActivityCommand(Guid.NewGuid(), Guid.NewGuid(),"New activity", dailyProductivity.Day);

        _dailyProductivityRepository
            .GetByDateAsync(command.Day)
            .Returns(dailyProductivity);;
        
        //act
        await Act(command);
        
        //assert
        await _dailyProductivityRepository
            .Received(1)
            .UpdateAsync(dailyProductivity);

        await _dailyProductivityRepository
            .Received(0)
            .AddAsync(Arg.Any<DailyProductivity>());

        dailyProductivity
            .Activities
            .Any(x => x.Id.Equals(command.Id) && x.Title == command.Title)
            .ShouldBeTrue();
    }
    
    #region arrange
    private readonly IDailyProductivityRepository _dailyProductivityRepository;
    private readonly ICommandHandler<CreateActivityCommand> _handler;

    public CreateActivityCommandHandlerTests()
    {
        _dailyProductivityRepository = Substitute.For<IDailyProductivityRepository>();
        _handler = new CreateActivityCommandHandler(_dailyProductivityRepository);
    }
    #endregion
}