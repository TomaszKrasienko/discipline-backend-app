using System.Text;
using Microsoft.Extensions.Options;

namespace discipline.centre.shared.infrastructure.Auth.Configuration;

/// <summary>
/// Validator for <see cref="JwtOptions"/>
/// Checks for string required fields for null values
/// Checks for TimeSpan fields to ensure they are not null
/// </summary>
internal sealed class JwtOptionsValidator : IValidateOptions<JwtOptions>
{
    public ValidateOptionsResult Validate(string? name, JwtOptions options)
    {
        StringBuilder errorMessagesBuilder = new();
        foreach (var authorizeParams in options.AuthorizeParams)
        {
            var authorizeResults = ValidateAuthorizeParams(authorizeParams.Value);
            
            if (authorizeResults is not null)
            {
                errorMessagesBuilder.Append(authorizeParams);
            }
        }

        return errorMessagesBuilder.Length == 0 
            ? ValidateOptionsResult.Success 
            : ValidateOptionsResult.Fail(errorMessagesBuilder.ToString());
    }

    private static string? ValidateAuthorizeParams(AuthorizeParamsOptions options)
    {
        List<(bool, string?)> stringAuthorizeParamsValidationResults =
        [
            ValidateIfStringIsEmpty(options.PublicCertPath, nameof(options.PublicCertPath)),
            ValidateIfStringIsEmpty(options.Issuer, nameof(options.Issuer)),
            ValidateIfStringIsEmpty(options.Audience, nameof(options.Audience)),
        ];

        if (stringAuthorizeParamsValidationResults.TrueForAll(x => x.Item1))
        {
            return null;
        }
        
        var errorMessagesBuilder = new StringBuilder();
        errorMessagesBuilder.Append(stringAuthorizeParamsValidationResults
            .Where(x => !x.Item1)
            .Select(x => $"{x.Item2}, "));
        
        return errorMessagesBuilder.ToString();
    } 

    private static (bool, string?) ValidateIfStringIsEmpty(string value, string fieldName)
        => string.IsNullOrWhiteSpace(value) 
            ? (false, $"The field {fieldName} cannot be empty") 
            : (true, null);
}