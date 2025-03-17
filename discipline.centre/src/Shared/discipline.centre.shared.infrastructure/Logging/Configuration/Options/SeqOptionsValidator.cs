using Microsoft.Extensions.Options;

namespace discipline.centre.shared.infrastructure.Logging.Configuration.Options;

internal sealed class SeqOptionsValidator : IValidateOptions<SeqOptions>
{
    public ValidateOptionsResult Validate(string? name, SeqOptions options)
        => string.IsNullOrWhiteSpace(options.Url)
            ? ValidateOptionsResult.Fail("Seq Url name is required") 
            : ValidateOptionsResult.Success;
}