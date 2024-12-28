using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.CQRS.Queries;

namespace discipline.centre.activityrules.application.ActivityRules.Queries;

public sealed record GetActiveModesByDayQuery(DateOnly Day) : IQuery<ActiveModesDto>;

internal sealed class GetActiveModesByDayQueryHandler : IQueryHandler<GetActiveModesByDayQuery, ActiveModesDto>
{
    public Task<ActiveModesDto> HandleAsync(GetActiveModesByDayQuery query, CancellationToken cancellationToken = default)
    {
        var activeModesDto = new ActiveModesDto
        {
            Values = [Mode.EveryDayMode]
        };
        
        var dayOfWeek = query.Day.DayOfWeek;
        
        switch (dayOfWeek)
        {
            case DayOfWeek.Monday:
                activeModesDto.Values.Add(Mode.FirstDayOfWeekMode);
                break;
            case DayOfWeek.Sunday:
                activeModesDto.Values.Add(Mode.LastDayOfWeekMode);
                break;
        }

        var firstDayOfNextMonth = new DateOnly(
            query.Day.Month == 12 ? query.Day.Year + 1 : query.Day.Year, 
            query.Day.Month == 12 ? 1 :query.Day.Month + 1,
            1);
        var lastDayOfMonth = firstDayOfNextMonth.AddDays(-1).Day;

        if(query.Day.Day == 1)
        {
            activeModesDto.Values.Add(Mode.FirstDayOfMonth);
        }
        if(query.Day.Day == lastDayOfMonth)
        {
            activeModesDto.Values.Add(Mode.LastDayOfMonthMode);
        }
        
        activeModesDto.Values.Add((query.Day.DayOfWeek == 0 ? 7 : (int)query.Day.DayOfWeek).ToString());
        
        return Task.FromResult(activeModesDto);
    }
}