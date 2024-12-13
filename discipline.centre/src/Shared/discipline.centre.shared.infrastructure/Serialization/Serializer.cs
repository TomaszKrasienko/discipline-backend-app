using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using discipline.centre.shared.abstractions.Serialization;

namespace discipline.centre.shared.infrastructure.Serialization;

internal sealed class Serializer : ISerializer
{
    private readonly JsonSerializerOptions _options = new ()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public string ToJson<T>(T value) where T : class
        => JsonSerializer.Serialize(value, _options);

    public byte[] ToByteJson<T>(T value) where T : class
        => Encoding.UTF8.GetBytes(ToJson(value));

    public object? ToObject(string json, Type type)
        => JsonSerializer.Deserialize(json, type, _options);

    public T? ToObject<T>(string json) where T : class
        => JsonSerializer.Deserialize<T>(json, _options);
}