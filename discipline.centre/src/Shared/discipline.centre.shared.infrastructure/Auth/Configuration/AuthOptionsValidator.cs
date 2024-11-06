using Microsoft.Extensions.Options;

namespace discipline.centre.shared.infrastructure.Auth.Configuration;

internal sealed class AuthOptionsValidator : IValidateOptions<AuthOptions>
{
    public ValidateOptionsResult Validate(string? name, AuthOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.PublicCertPath))
        {
            return ValidateOptionsResult.Fail("Auth options public cert path can not be empty");
        }

        if (string.IsNullOrWhiteSpace(options.PrivateCertPath))
        {
            return ValidateOptionsResult.Fail("Auth options private cert path can not be empty");
        }

        if (string.IsNullOrWhiteSpace(options.Password))
        {
            return ValidateOptionsResult.Fail("Auth options password can not be empty");
        }

        if (string.IsNullOrWhiteSpace(options.Issuer))
        {
            return ValidateOptionsResult.Fail("Auth options issuer can not be empty");
        }

        if (string.IsNullOrWhiteSpace(options.Audience))
        {
            return ValidateOptionsResult.Fail("Auth options audience can not be empty");
        }

        if (options.Expiry == default)
        {
            return ValidateOptionsResult.Fail("Auth options expiry can not be default");
        }

        return ValidateOptionsResult.Success;
    }
}