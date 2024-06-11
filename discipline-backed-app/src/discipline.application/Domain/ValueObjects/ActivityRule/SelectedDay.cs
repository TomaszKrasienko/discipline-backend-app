using discipline.application.Domain.Exceptions;

namespace discipline.application.Domain.ValueObjects.ActivityRule;

internal sealed record SelectedDay
{
    public bool IsChecked { get; }
    public int DayOfWeek { get; }

    public SelectedDay(bool isChecked, int dayOfWeek)
    {
        if (dayOfWeek < 1 || dayOfWeek > 7)
        {
            throw new SelectedDayOutOfRangeException(dayOfWeek);
        }
        IsChecked = isChecked;
        DayOfWeek = dayOfWeek;
    }
}