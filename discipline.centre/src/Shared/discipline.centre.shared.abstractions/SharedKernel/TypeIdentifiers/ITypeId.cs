namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

//Marker
public interface ITypeId<out TType> where TType : class
{
    static abstract TType New();
    static abstract TType Parse(string stringTypedId);
}