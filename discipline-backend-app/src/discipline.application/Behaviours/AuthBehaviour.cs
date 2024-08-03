using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using discipline.application.Configuration;
using discipline.application.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace discipline.application.Behaviours;

internal static class AuthBehaviour
{
    private const string SectionName = "auth";
    
    internal static IServiceCollection AddAuthBehaviour(this IServiceCollection services,
        IConfiguration configuration)
        => services
                .AddTokenValidation(configuration)
                .AddServices()
                .AddOptions(configuration);
    
    private static IServiceCollection AddTokenValidation(this IServiceCollection services, IConfiguration configuration)
    {
        var authOptions = configuration.GetOptions<AuthOptions>(SectionName);
        RSA privateRsa = RSA.Create();
        privateRsa.ImportFromEncryptedPem(File.ReadAllText(authOptions.PrivateCertPath), authOptions.Password);
        RSA publicRsa = RSA.Create();
        publicRsa.ImportFromPem(File.ReadAllText(authOptions.PublicCertPath));
        var publicKey = new RsaSecurityKey(publicRsa);

        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = authOptions.Issuer,
                    ValidAudience = authOptions.Audience,
                    IssuerSigningKey = publicKey
                };
            });
        
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services.AddSingleton<IAuthenticator, JwtAuthenticator>();
    
    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<AuthOptions>(configuration.GetSection(SectionName));
    
    internal static WebApplication UseAuthBehaviour(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}

public sealed record AuthOptions
{
    public string PublicCertPath { get; init; }
    public string PrivateCertPath { get; init; }
    public string Password { get; set; }
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public TimeSpan Expiry { get; init; }
}

internal interface IAuthenticator
{
    JwtDto CreateToken(string userId, string status);
}

internal sealed class JwtAuthenticator : IAuthenticator
{
    private readonly IClock _clock;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _privateCertPath;
    private readonly string _password;
    private readonly TimeSpan _expiry;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public JwtAuthenticator(IClock clock, IOptions<AuthOptions> options)
    {
        _clock = clock;
        _issuer = options.Value.Issuer;
        _audience = options.Value.Audience;
        _privateCertPath = options.Value.PrivateCertPath;
        _password = options.Value.Password;
        _expiry = options.Value.Expiry;
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }
    
    public JwtDto CreateToken(string userId, string status)
    {
        RSA privateRsa = RSA.Create();
        privateRsa.ImportFromEncryptedPem(input: File.ReadAllText(_privateCertPath), password: _password);
        var privateKey = new RsaSecurityKey(privateRsa);
        var signingCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, userId),
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim("status", status)
        };

        var now = _clock.DateNow();
        var expirationTime = now.Add(_expiry);

        var jwt = new JwtSecurityToken(
            issuer: _issuer, 
            audience: _audience,
            claims: claims,
            notBefore: now,
            expires: expirationTime,
            signingCredentials: signingCredentials);
        var token = _jwtSecurityTokenHandler.WriteToken(jwt);
        return new JwtDto()
        {
            Token = token
        };
    }
}