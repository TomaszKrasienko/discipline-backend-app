namespace discipline.domain.SharedKernel.TypeIdentifiers;

//Marker
public interface ITypeId<TType> where TType : class
{
    static abstract TType New();
}