using Microsoft.Extensions.Options;

namespace discipline.centre.shared.infrastructure.Configuration;

internal sealed class AppOptionsValidator : IValidateOptions<AppOptions>
{
    public ValidateOptionsResult Validate(string? name, AppOptions options)
    {
        if (string.IsNullOrWhiteSpace(options?.Name))
        {
            return ValidateOptionsResult.Fail("App name string can not be null or empty");
        }
        return ValidateOptionsResult.Success;
    }
}