using System.Text;
using Microsoft.Extensions.Options;

namespace discipline.centre.users.infrastructure.Users.Auth.Configuration.Options;

internal sealed class RefreshTokenOptionsValidator : IValidateOptions<RefreshTokenOptions>
{
    public ValidateOptionsResult Validate(string? name, RefreshTokenOptions options)
    {
        var errorMessageBuilder = new StringBuilder();

        if (options.Length <= 0)
        {
            errorMessageBuilder.Append("Length must be positive number, ");
        }

        if (options.Expiry == TimeSpan.Zero)
        {
            errorMessageBuilder.Append("Expiry cannot be zero, ");
        }
        
        return errorMessageBuilder.Length == 0
            ? ValidateOptionsResult.Success 
            : ValidateOptionsResult.Fail(errorMessageBuilder.ToString());
    }
}