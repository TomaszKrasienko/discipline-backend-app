using Microsoft.Extensions.Options;

namespace discipline.centre.shared.infrastructure.Cache.Configuration;

internal sealed class RedisCacheOptionsValidator : IValidateOptions<RedisCacheOptions>
{
    public ValidateOptionsResult Validate(string? name, RedisCacheOptions options)
    {
        if (string.IsNullOrWhiteSpace(options?.ConnectionString))
        {
            return ValidateOptionsResult.Fail("Redis Cache connection string can not be null or empty");
        }
        return ValidateOptionsResult.Success;
    }
}