namespace discipline.application.Behaviours;

//TODO Przeniesienie to infra
internal static class ConvertingToQuartzExpressionBehaviour
{
    private const string EmptySign = "*";

    internal static string AsQuartzExpression(this TimeOnly time)
        => $"{GetNumberOrEmptySign(time.Second)} {GetNumberOrEmptySign(time.Minute)} {GetNumberOrEmptySign(time.Hour)} * * ?";

    private static string GetNumberOrEmptySign(int number)
        => number == 0 ? "*" : number.ToString();
}