namespace discipline.application.Domain.Services.Abstractions;

public interface IWeekdayCheckService
{
    bool IsDateForMode(DateTime now, string mode, List<int> selectedDays = null);
}