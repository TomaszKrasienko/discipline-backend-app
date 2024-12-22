namespace discipline.centre.users.infrastructure.Users.Auth.Configuration.Options;

internal sealed class RefreshTokenOptions
{
    public int Length { get; init; }
    public TimeSpan Expiry { get; init; }
}