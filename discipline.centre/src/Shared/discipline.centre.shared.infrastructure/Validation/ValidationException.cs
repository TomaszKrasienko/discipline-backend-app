using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.shared.infrastructure.Validation;

public class ValidationException(string code, string message, IDictionary<string, string[]> validationParams) 
    : DisciplineException(code, message)
{
    public IDictionary<string, string[]> ValidationParams => validationParams;
}