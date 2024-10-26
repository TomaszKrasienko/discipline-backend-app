using discipline.application.Configuration;
using discipline.domain.Configuration;
using discipline.infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddDomain()
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration);
builder
    .UseApplication();
var app = builder.Build();
app.UseRouting();
app.UseApplication();
app.UseHttpsRedirection();
app.Run();

