using System.Linq.Expressions;

namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public interface IBaseTypeId<out TType, out TValue>
    where TType : class, IBaseTypeId<TType, TValue>
    where TValue : struct
{
    TValue Value { get; }
}

public interface ITypeId<out TType, out TValue> : IBaseTypeId<TType, TValue> 
    where TType : class, IBaseTypeId<TType, TValue> 
    where TValue : struct
{
    static abstract TType New();
    static abstract TType Parse(string stringTypedId);
}