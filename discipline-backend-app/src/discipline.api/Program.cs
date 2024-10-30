using discipline.application.Configuration;
using discipline.domain.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddDomain()
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration);
var app = builder.Build();
app.UseRouting();
app.UseApplication();
app.UseHttpsRedirection();
await app.RunAsync();

