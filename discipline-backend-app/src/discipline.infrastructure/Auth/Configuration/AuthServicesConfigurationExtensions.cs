using System.Security.Cryptography;
using discipline.application.Behaviours.Auth;
using discipline.infrastructure.Auth;
using discipline.infrastructure.Auth.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class AuthServicesConfigurationExtensions
{
    
    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions(configuration);
        services.AddTokenValidation();
        services.AddUserStateChecking();
        return services;
    }
    
    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<AuthOptions>(configuration.GetSection(nameof(AuthOptions)));
    
    private static IServiceCollection AddTokenValidation(this IServiceCollection services)
    {
        var authOptions = services.GetOptions<AuthOptions>().Value;
        var validationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = authOptions.Issuer,
            ValidAudience = authOptions.Audience,
            LogValidationExceptions = true,
        };
        
        RSA publicRsa = RSA.Create();
        publicRsa.ImportFromPem(File.ReadAllText(authOptions.PublicCertPath));
        var publicKey = new RsaSecurityKey(publicRsa);
        validationParameters.IssuerSigningKey = publicKey;


        services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = validationParameters;
            });
        
        return services;
    }
    
    internal static IServiceCollection AddUserStateChecking(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(UserStatePolicy.Name, policy =>
            {
                policy.Requirements.Add(new UserStateRequirement());
            });
        });
        services
            .AddSingleton<IAuthorizationHandler, UserStateAuthorizationHandler>();
        return services;
    }
}

