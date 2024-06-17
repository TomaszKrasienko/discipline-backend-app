using discipline.application.Features.Base.Abstractions;
using discipline.application.Features.Configuration.Base.Abstractions;
using FluentValidation;

namespace discipline.application.Features.ActivityRules;

public class EditActivityRule
{
    
}

public sealed record EditActivityRuleCommand(Guid Id, string Title, string Mode, 
    List<int> SelectedDays) : ICommand;

public sealed class EditActivityRuleCommandValidator : AbstractValidator<EditActivityRuleCommand>
{
    public EditActivityRuleCommandValidator()
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

internal sealed class EditActivityRuleCommandHandler : ICommandHandler<EditActivityRuleCommand>
{
    public async Task HandleAsync(EditActivityRuleCommand command, CancellationToken cancellationToken = default)
    {
        
    }
}