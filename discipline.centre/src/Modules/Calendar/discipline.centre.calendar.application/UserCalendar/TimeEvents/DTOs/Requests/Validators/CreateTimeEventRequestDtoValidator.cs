using FluentValidation;

namespace discipline.centre.calendar.application.UserCalendar.TimeEvents.DTOs.Requests.Validators;

internal sealed class CreateTimeEventRequestDtoValidator : AbstractValidator<CreateTimeEventRequestDto>
{
    public CreateTimeEventRequestDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Time event title cannot be blank");
    }
}