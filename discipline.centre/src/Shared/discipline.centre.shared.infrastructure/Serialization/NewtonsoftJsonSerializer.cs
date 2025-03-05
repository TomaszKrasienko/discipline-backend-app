using System.Text;
using discipline.centre.shared.abstractions.Serialization;
using Newtonsoft.Json;

namespace discipline.centre.shared.infrastructure.Serialization;

internal sealed class NewtonsoftJsonSerializer : ISerializer
{
    public string ToJson<T>(T value) where T : class
        => JsonConvert.SerializeObject(value);

    public byte[] ToByteJson<T>(T value) where T : class
        => Encoding.UTF8.GetBytes(ToJson(value));

    public object? ToObject(string json, Type type)
        => JsonConvert.DeserializeObject(json, type);

    public T? ToObject<T>(string json) where T : class
        => JsonConvert.DeserializeObject<T>(json);
}