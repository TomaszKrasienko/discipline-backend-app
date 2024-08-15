namespace discipline.domain.DailyProductivities.Services.Abstractions;

public interface IWeekdayCheckService
{
    static abstract IWeekdayCheckService GetInstance();
    bool IsDateForMode(DateTime now, string mode, List<int> selectedDays = null);
}