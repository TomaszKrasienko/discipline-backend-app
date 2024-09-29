using discipline.application.Features.UsersCalendars;
using FluentValidation;
using FluentValidation.TestHelper;

namespace discipline.application.unit_tests.Features.UsersCalendars.ChangeEventDate;

public sealed class ChangeEventDateCommandValidatorTests
{
    private TestValidationResult<ChangeEventDateCommand> Act(ChangeEventDateCommand command)
        => _validator.TestValidate(command);
    
    
    #region arrange
    private readonly IValidator<ChangeEventDateCommand> _validator;

    public ChangeEventDateCommandValidatorTests()
        => _validator = new ChangeEventDateCommandValidator();
    #endregion
}