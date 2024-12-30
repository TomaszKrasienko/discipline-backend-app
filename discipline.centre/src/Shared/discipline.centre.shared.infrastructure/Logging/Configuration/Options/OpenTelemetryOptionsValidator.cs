using Microsoft.Extensions.Options;

namespace discipline.centre.shared.infrastructure.Logging.Configuration.Options;

internal sealed record OpenTelemetryOptionsValidator : IValidateOptions<OpenTelemetryOptions>
{
    public ValidateOptionsResult Validate(string? name, OpenTelemetryOptions options)
        => string.IsNullOrWhiteSpace(options.InternalSourceName) 
            ? ValidateOptionsResult.Fail("Internal source name is required") 
            : ValidateOptionsResult.Success;
}