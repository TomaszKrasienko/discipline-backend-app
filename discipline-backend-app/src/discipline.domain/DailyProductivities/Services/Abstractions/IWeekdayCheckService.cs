namespace discipline.domain.DailyProductivities.Services.Abstractions;

public interface IWeekdayCheckService
{
    static abstract IWeekdayCheckService GetInstance();
    bool IsDateForMode(DateTimeOffset now, string mode, List<int> selectedDays = null);
}