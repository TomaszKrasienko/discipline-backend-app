namespace discipline.application.Behaviours.Time;

public interface IClock
{
    DateOnly DateNow();
    DateTimeOffset DateTimeNow();
}