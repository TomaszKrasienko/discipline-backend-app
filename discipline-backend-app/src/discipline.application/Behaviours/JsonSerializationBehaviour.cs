using Newtonsoft.Json;

namespace discipline.application.Behaviours;

public static class JsonSerializationBehaviour
{
    public static string AsJson(this object data)
        => JsonConvert.SerializeObject(data);
}
