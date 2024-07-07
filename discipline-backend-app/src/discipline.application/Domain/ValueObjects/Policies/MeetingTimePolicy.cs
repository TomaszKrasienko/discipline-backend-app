using System.Dynamic;
using discipline.application.Domain.Exceptions;
using discipline.application.Domain.ValueObjects.Policies.Abstractions;

namespace discipline.application.Domain.ValueObjects.Policies;

internal sealed class MeetingTimePolicy : IPolicy
{
    private readonly TimeOnly _timeFrom;
    private readonly TimeOnly _timeTo;
    
    internal static MeetingTimePolicy GetInstance(TimeOnly timeFrom, TimeOnly timeTo)
        => new MeetingTimePolicy(timeFrom, timeTo);

    private MeetingTimePolicy(TimeOnly timeFrom, TimeOnly timeTo)
    {
        _timeFrom = timeFrom;
        _timeTo = timeTo;
    }

    public void Validate()
    {
        if (_timeFrom >= _timeTo)
        {
            throw new InvalidMeetingTimeSpanException(_timeFrom, _timeTo);
        }
    }
}