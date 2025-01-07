using System.Linq.Expressions;

namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public interface ITypeId<out TType, TValue> where TType : class where TValue : struct
{
    TValue Value { get; }
    static abstract TType Create();
    static abstract TType Create(TValue value);
    static abstract TType Parse(string stringTypedId);
    string GetString() => Value!.ToString()!;
}