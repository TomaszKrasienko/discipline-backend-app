namespace discipline.centre.users.infrastructure.Users.RefreshToken.Configuration;

internal sealed class RefreshTokenOptions
{
    public int Length { get; init; }
    public TimeSpan Expiry { get; init; }
}