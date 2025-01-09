using System.Text.Json;
using System.Text.Json.Serialization;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.shared.infrastructure.Converters;

public class TypeIdJsonConverter<TIdentifier, TValue> : JsonConverter<IBaseTypeId<TIdentifier, TValue>> 
    where TIdentifier : class, ITypeId<TIdentifier, TValue> 
    where TValue : struct
{
    public override IBaseTypeId<TIdentifier, TValue>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, IBaseTypeId<TIdentifier, TValue> value, JsonSerializerOptions options)
    {
        // JsonSerializer.Serialize(writer, value.Value, options);
        writer.WriteStringValue(value.Value.ToString());
    }
}