namespace discipline.api.integration_tests._Helpers;

internal static class TestsEnvironmentProvider
{
    internal static string GetEnvironments()
        => (Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "docker.tests") == "Development"
            ? "tests" : Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
}