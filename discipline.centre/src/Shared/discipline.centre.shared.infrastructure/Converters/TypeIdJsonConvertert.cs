using System.Text.Json;
using System.Text.Json.Serialization;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.shared.infrastructure.Converters;

public class TypeIdJsonConverter<TIdentifier, TValue> : JsonConverter<TIdentifier>
    where TIdentifier : IBaseTypeId<TValue>
    where TValue : struct
{
    public override TIdentifier? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        //TODO: This should stay like that
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, TIdentifier value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
