using Microsoft.Extensions.Options;

namespace discipline.centre.shared.infrastructure.Events.Brokers.Redis.Configuration;

internal sealed class RedisBrokerOptionsValidator : IValidateOptions<RedisBrokerOptions>
{
    public ValidateOptionsResult Validate(string? name, RedisBrokerOptions options)
    {
        if (string.IsNullOrWhiteSpace(options?.ConnectionString))
        {
            return ValidateOptionsResult.Fail("Redis Broker connection string can not be null or empty");
        }
        return ValidateOptionsResult.Success;
    }
}