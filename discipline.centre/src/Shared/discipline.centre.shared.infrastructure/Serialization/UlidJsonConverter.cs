using System.Text.Json;
using System.Text.Json.Serialization;

namespace discipline.centre.shared.infrastructure.Serialization;

internal sealed class UlidJsonConverter : JsonConverter<Ulid>
{
    public override Ulid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => Ulid.Parse(reader.GetString()!);

    public override void Write(Utf8JsonWriter writer, Ulid value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString());
}