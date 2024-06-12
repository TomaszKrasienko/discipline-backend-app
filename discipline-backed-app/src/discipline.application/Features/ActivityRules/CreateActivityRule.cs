using discipline.application.Features.Base.Abstractions;
using FluentValidation;

namespace discipline.application.Features.ActivityRules;

public sealed record CreateActivityRuleCommand(Guid Id, string Title, string Mode,
    List<int> SelectedDays) : ICommand;

internal sealed class CreateActivityRuleCommandValidator : AbstractValidator<CreateActivityRuleCommand>
{
    public CreateActivityRuleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Activity rule \"ID\" can not be empty");
        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Activity rule \"Title\" can not be null or empty");
        RuleFor(x => x.Title)
            .MinimumLength(3)
            .MaximumLength(40)
            .WithMessage("Activity rule \"Title\" has invalid length");
        RuleFor(x => x.Mode)
            .NotNull()
            .NotEmpty()
            .WithMessage("Activity rule \"Mode\" can not be null or empty");
    }
}

internal sealed class CreateActivityRuleCommandHandler : ICommandHandler<CreateActivityRuleCommand>
{
    public Task HandleAsync(CreateActivityRuleCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
