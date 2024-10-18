namespace discipline.domain.SharedKernel;

//Marker
public interface ITypeId<TType> where TType : class
{
    static abstract TType New();
}