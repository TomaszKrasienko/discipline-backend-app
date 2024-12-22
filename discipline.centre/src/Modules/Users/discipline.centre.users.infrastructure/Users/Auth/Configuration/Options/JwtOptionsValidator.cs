using System.Text;
using Microsoft.Extensions.Options;

namespace discipline.centre.users.infrastructure.Users.Auth.Configuration.Options;

internal sealed class JwtOptionsValidator : IValidateOptions<JwtOptions>
{
    public ValidateOptionsResult Validate(string? name, JwtOptions options)
    {
        StringBuilder errorMessagesBuilder = new();
        List<(bool, string?)> stringKeyPublishingValidationResults =
        [
            ValidateIfStringIsEmpty(options.KeyPublishing.PrivateCertPath, nameof(options.KeyPublishing.PrivateCertPath)),
            ValidateIfStringIsEmpty(options.KeyPublishing.PrivateCertPassword, nameof(options.KeyPublishing.PrivateCertPassword)),
            ValidateIfStringIsEmpty(options.KeyPublishing.Issuer, nameof(options.KeyPublishing.Issuer)),
            ValidateIfStringIsEmpty(options.KeyPublishing.Audience, nameof(options.KeyPublishing.Audience)),
        ];

        if (!stringKeyPublishingValidationResults.TrueForAll(x => x.Item1))
        {
            errorMessagesBuilder.Append(stringKeyPublishingValidationResults
                .Where(x => !x.Item1)
                .Select(x => $"{x.Item2}, "));
        }

        if (options.KeyPublishing.TokenExpiry == TimeSpan.Zero)
        {
            errorMessagesBuilder.Append("The token expiry cannot be zero, ");
        }
        
        return errorMessagesBuilder.Length == 0 
            ? ValidateOptionsResult.Success 
            : ValidateOptionsResult.Fail(errorMessagesBuilder.ToString());
    }
    
    private static (bool, string?) ValidateIfStringIsEmpty(string value, string fieldName)
        => string.IsNullOrWhiteSpace(value) 
            ? (false, $"The field {fieldName} cannot be empty") 
            : (true, null);
}