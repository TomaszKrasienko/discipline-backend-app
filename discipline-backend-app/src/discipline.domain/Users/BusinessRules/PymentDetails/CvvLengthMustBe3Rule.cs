using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.PymentDetails;

internal sealed class CvvLengthMustBe3Rule(string cvvCode) : IBusinessRule
{
    public Exception Exception => new InvalidCvvLengthException(cvvCode);

    public bool IsBroken()
        => cvvCode.Length is not 3;
}