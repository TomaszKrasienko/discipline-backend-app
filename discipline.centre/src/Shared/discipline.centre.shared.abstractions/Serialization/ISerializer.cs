namespace discipline.centre.shared.abstractions.Serialization;

public interface ISerializer
{
    string ToJson<T>(T value) where T : class;
}