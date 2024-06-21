using discipline.application.Behaviours;
using discipline.application.Domain.Entities;
using discipline.application.Domain.Repositories;
using discipline.application.Features.Configuration.Base.Abstractions;
using FluentValidation;

namespace discipline.application.Features.DailyProductivities;

public class CreateActivity
{
    
}

internal sealed record CreateActivityCommand(Guid Id, string Title) : ICommand;

internal sealed class CreateActivityCommandValidator : AbstractValidator<CreateActivityCommand>
{
    public CreateActivityCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Activity \"ID\" can not be empty");

        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Activity \"Title\" can not be null or empty");

        RuleFor(x => x.Title)
            .MinimumLength(3)
            .MaximumLength(100)
            .WithMessage("Activity \"Title\" has invalid length");
    }
}

internal sealed class CreateActivityCommandHandler(
    IDailyProductivityRepository dailyProductivityRepository,
    IClock clock) : ICommandHandler<CreateActivityCommand>
{
    public async Task HandleAsync(CreateActivityCommand command, CancellationToken cancellationToken = default)
    {
        var now = clock.DateNow();
        var dailyProductivity = await dailyProductivityRepository.GetByDateAsync(now, cancellationToken);
        if (dailyProductivity is null)
        {
            dailyProductivity = DailyProductivity.Create(now);
        }
    }
}