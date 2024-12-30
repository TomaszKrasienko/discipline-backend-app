using Microsoft.Extensions.Options;

namespace discipline.centre.shared.infrastructure.Logging.Configuration.Options;

internal sealed class JaegerOptionsValidator : IValidateOptions<JaegerOptions>
{
    public ValidateOptionsResult Validate(string? name, JaegerOptions options)
        => string.IsNullOrWhiteSpace(options.Endpoint) 
            ? ValidateOptionsResult.Fail("Host is required") 
            : ValidateOptionsResult.Success;
}