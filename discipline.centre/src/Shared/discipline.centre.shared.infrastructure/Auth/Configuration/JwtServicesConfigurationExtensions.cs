using System.Security.Cryptography;
using discipline.centre.shared.infrastructure.Auth;
using discipline.centre.shared.infrastructure.Auth.Configuration;
using discipline.centre.shared.infrastructure.Auth.Const;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class JwtServicesConfigurationExtensions
{
    private const string DefaultAuthorizeParamsKey = "Default";
    private const string HangfireAuthorizeParamsKey = "Hangfire";
    
    internal static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
        => services
            .ValidateAndAddOptions(configuration)
            .AddTokenValidation()
            .AddUserStateChecking();

    private static IServiceCollection ValidateAndAddOptions(this IServiceCollection services,
        IConfiguration configuration)
        => services.ValidateAndBind<JwtOptions, JwtOptionsValidator>(configuration);
    
    private static IServiceCollection AddTokenValidation(this IServiceCollection services)
    {
        var authOptions = services.GetOptions<JwtOptions>();
        
        if(!authOptions.AuthorizeParams.TryGetValue(DefaultAuthorizeParamsKey, out var defaultAuthorizeParams))
        {
            throw new InvalidOperationException("Authorize parameters for 'Default' are missing.");
        }
        
        var defaultValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = defaultAuthorizeParams.Issuer,
            ValidAudience = defaultAuthorizeParams.Audience,
            LogValidationExceptions = true,
            IssuerSigningKey = GetRsaSecurityKey(defaultAuthorizeParams.PublicCertPath)
        };

        if (!authOptions.AuthorizeParams.TryGetValue(HangfireAuthorizeParamsKey, out var hangfireAuthorizeParams))
        {
            throw new InvalidOperationException("Authorize parameters for 'Hangfire' are missing.");
        }
        
        var internalValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = hangfireAuthorizeParams.Issuer,
            ValidAudience = hangfireAuthorizeParams.Audience,
            LogValidationExceptions = true,
            IssuerSigningKey = GetRsaSecurityKey(hangfireAuthorizeParams.PublicCertPath)
        };

        services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = defaultValidationParameters;
            })
            .AddJwtBearer(AuthorizationSchemes.HangfireAuthorizeSchema, options =>
            {
                options.TokenValidationParameters = internalValidationParameters;
            });
        
        return services;
    }

    private static RsaSecurityKey GetRsaSecurityKey(string path)
    {
        RSA publicInternalRsa = RSA.Create();
        publicInternalRsa.ImportFromPem(File.ReadAllText(path));
        return new RsaSecurityKey(publicInternalRsa);
    }
    
    private static IServiceCollection AddUserStateChecking(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(UserStatePolicy.Name, policy =>
            {
                policy.Requirements.Add(new UserStateRequirement());
            });
        });
        return services;
    }
}