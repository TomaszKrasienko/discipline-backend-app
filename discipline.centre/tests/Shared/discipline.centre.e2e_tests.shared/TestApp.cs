using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.e2e_tests.shared;

internal sealed class TestApp : WebApplicationFactory<Program>
{
    public HttpClient HttpClient { get; private set; }

    public TestApp(Action<IServiceCollection>? services)
    {
        HttpClient = WithWebHostBuilder(builder =>
        {
            if (services is not null)
            {
                builder.ConfigureServices(services);
            }

            builder.UseEnvironment("tests");
        }).CreateClient();
    }
}