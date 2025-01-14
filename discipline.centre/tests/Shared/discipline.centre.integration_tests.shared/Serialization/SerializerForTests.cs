using System.Text.Json;
using System.Text.Json.Serialization;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.Converters;

namespace discipline.centre.integration_tests.shared.Serialization;

public static class SerializerForTests
{
    public static T? Deserialize<T>(string json)
    {
        var baseTypeIdInterface = typeof(IBaseTypeId<>);
            
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        var types = assemblies
            .SelectMany(x => x.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(x => x.GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == baseTypeIdInterface)
                .Select(y => new {ImplementationType = x, InterfaceType = y}));

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
        
        foreach (var type in types)
        {
            var implementationType = type.ImplementationType;
            var interfaceType = type.InterfaceType;
                
            var genericArguments = interfaceType.GetGenericArguments();
            var tValue = genericArguments[0];
                
            var converterType = typeof(TypeIdJsonConverter<,>).MakeGenericType(implementationType, tValue);
            var converter = Activator.CreateInstance(converterType);
                
            if (converter is JsonConverter jsonConverter)
            {
                options.Converters.Add(jsonConverter);
            }
        }
        
        return JsonSerializer.Deserialize<T>(json, options);
    }
}