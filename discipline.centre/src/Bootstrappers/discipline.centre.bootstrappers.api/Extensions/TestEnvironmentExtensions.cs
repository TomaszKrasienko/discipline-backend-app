namespace discipline.centre.bootstrappers.api.Extensions;

public static class TestEnvironmentExtensions
{
    public const string TestEnvironment = "tests";
    
     public static bool IsTestsEnvironment(this IHostEnvironment hostEnvironment)
        => hostEnvironment.IsEnvironment("tests");
}