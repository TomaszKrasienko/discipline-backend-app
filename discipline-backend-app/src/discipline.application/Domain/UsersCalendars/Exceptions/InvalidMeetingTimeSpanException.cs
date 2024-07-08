using discipline.application.Exceptions;

namespace discipline.application.Domain.Exceptions;

public sealed class InvalidMeetingTimeSpanException(TimeOnly timeFrom, TimeOnly timeTo) 
    : DisciplineException($"Meeting time span with time from: {timeFrom} and time to: {timeTo} is invalid");