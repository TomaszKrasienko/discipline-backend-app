using Newtonsoft.Json;

namespace discipline.infrastructure.Serialization;

//TODO Do poprawy
public static class JsonSerializationBehaviour
{
    public static string AsJson(this object data)
        => JsonConvert.SerializeObject(data);
}
