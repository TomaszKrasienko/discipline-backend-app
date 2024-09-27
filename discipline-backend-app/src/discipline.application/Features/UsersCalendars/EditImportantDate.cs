using discipline.application.Behaviours;
using discipline.domain.UsersCalendars.Repositories;
using FluentValidation;

namespace discipline.application.Features.UsersCalendars;

internal static class EditImportantDate
{
    
}

public sealed record EditImportantDateCommand(Guid UserId, Guid Id, string Title) : ICommand;

public sealed class EditImportantDateCommandValidator : AbstractValidator<EditImportantDateCommand>
{
    public EditImportantDateCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Important date \"ID\" can not be empty");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Important date \"UserId\" can not be empty");

        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Important date \"Title\" can not be null or empty");

        RuleFor(x => x.Title)
            .MinimumLength(3)
            .MaximumLength(100)
            .WithMessage("Important date \"Title\" has invalid length");
    }
}

internal sealed class EditImportantDateCommandHandler(
    IUserCalendarRepository userCalendarRepository) : ICommandHandler<EditImportantDateCommand>
{
    public Task HandleAsync(EditImportantDateCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}