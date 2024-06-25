using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.api.integration_tests._Helpers;

internal sealed class TestApp : WebApplicationFactory<Program>
{
    public HttpClient HttpClient { get; set; }

    public TestApp(Action<IServiceCollection> services)
    {
        HttpClient = WithWebHostBuilder(builder =>
        {
            if (services is not null)
            {
                builder.ConfigureServices(services);
            }
            builder.UseEnvironment("docker.tests");
        }).CreateClient();
    }
}