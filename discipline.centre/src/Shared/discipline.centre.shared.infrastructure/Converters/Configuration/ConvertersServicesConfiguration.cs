using System.Reflection;
using System.Text.Json.Serialization;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.shared.infrastructure.Converters.Configuration;

internal static class ConvertersServicesConfiguration
{
    internal static IServiceCollection AddConverters(this IServiceCollection services, IList<Assembly> assemblies)
        => services.Configure<JsonOptions>(options =>
        {
            var baseTypeIdInterface = typeof(IBaseTypeId<,>);

            var types = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .SelectMany(x => x.GetInterfaces()
                    .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == baseTypeIdInterface)
                    .Select(y => new {ImplementationType = x, InterfaceType = y}));

            foreach (var type in types)
            {
                var interfaceType = type.InterfaceType;
                
                var genericArguments = interfaceType.GetGenericArguments();
                var tType = genericArguments[0];
                var tValue = genericArguments[1];
                
                var converterType = typeof(TypeIdJsonConverter<,>).MakeGenericType(tType, tValue);
                var converter = Activator.CreateInstance(converterType);
                
                if (converter is JsonConverter jsonConverter)
                {
                    options.SerializerOptions.Converters.Add(jsonConverter);
                }
            }
            options.SerializerOptions.Converters.Add(new TypeIdJsonConverter<ActivityRuleId, Ulid>());
        });
}