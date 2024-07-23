using discipline.application.Behaviours;
using discipline.application.Domain.Users.Enums;
using FluentValidation;

namespace discipline.application.Features.Users;

public class CreateUserSubscriptionOrder
{
    
}

public sealed record CreateUserSubscriptionOrderCommand(Guid UserId, Guid Id, 
    Guid SubscriptionId, SubscriptionOrderFrequency? SubscriptionOrderFrequency,
    string CardNumber, string CardCvvNumber) : ICommand;

public sealed class CreateUserSubscriptionOrderCommandValidator : AbstractValidator<CreateUserSubscriptionOrderCommand>
{
    public CreateUserSubscriptionOrderCommandValidator()
    {
        
    }
}