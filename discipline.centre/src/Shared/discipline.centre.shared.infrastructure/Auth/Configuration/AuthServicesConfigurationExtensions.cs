using System.Security.Cryptography;
using System.Text;
using discipline.centre.shared.infrastructure.Auth;
using discipline.centre.shared.infrastructure.Auth.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class AuthServicesConfigurationExtensions
{
    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        => services
            .ValidateAndAddOptions(configuration)
            .AddTokenValidation()
            .AddUserStateChecking();

    private static IServiceCollection ValidateAndAddOptions(this IServiceCollection services,
        IConfiguration configuration)
        => services.ValidateAndBind<AuthOptions, AuthOptionsValidator>(configuration);
    
    private static IServiceCollection AddTokenValidation(this IServiceCollection services)
    {
        var authOptions = services.GetOptions<AuthOptions>();
        var feValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = authOptions.Issuer,
            ValidAudience = authOptions.Audience,
            LogValidationExceptions = true,
        };
        
        var internalValidationParameters = new TokenValidationParameters()
        {
            //TODO: Add parameters
        };

        RSA publicRsa = RSA.Create();
        publicRsa.ImportFromPem(File.ReadAllText(authOptions.PublicCertPath));
        var publicKey = new RsaSecurityKey(publicRsa);
        feValidationParameters.IssuerSigningKey = publicKey;
        
        RSA publicInternalRsa = RSA.Create();
        publicInternalRsa.ImportFromPem(File.ReadAllText(authOptions.PublicInternalCertPath));
        var internalPublicKey = new RsaSecurityKey(publicInternalRsa);
        internalValidationParameters.IssuerSigningKey = internalPublicKey;


        services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = feValidationParameters;
            })
            .AddJwtBearer("Bearer_hf", options =>
            {
                options.TokenValidationParameters = internalValidationParameters;
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
        return services;
    }
}